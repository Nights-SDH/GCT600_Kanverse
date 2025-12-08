using UnityEngine;
using Photon.Pun;

public class MRUKLaserInteractor : MonoBehaviour
{
    [Header("Controller Settings")]
    public OVRInput.Controller controllerNode = OVRInput.Controller.RTouch;
    public OVRInput.Button grabButton = OVRInput.Button.PrimaryIndexTrigger;

    [Header("Ray Settings")]
    public float maxDistance = 5.0f;
    public LayerMask cardLayer;
    public LayerMask canvasLayer;

    [Header("References")]
    public LineRenderer lineRenderer; 
    
    private NetworkCard hoveredCard = null;
    private NetworkCard selectedObject = null;
    private bool isDragging = false;

    // [추가] 네트워크 전송 빈도 조절용 변수
    private float lastSendTime = 0f;
    private float sendInterval = 0.05f; // 0.05초마다 전송 (초당 약 20회)

    void Start()
    {
        if (lineRenderer != null)
        {
            lineRenderer.useWorldSpace = false;
            lineRenderer.SetPosition(0, Vector3.zero);
        }
    }

    void Update()
    {
        if (isDragging)
        {
            HandleDragging();
        }
        else
        {
            HandleHovering();
        }
    }

    void HandleHovering()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, cardLayer))
        {
            NetworkCard hitCard = hit.collider.GetComponent<NetworkCard>();

            if (hoveredCard != hitCard)
            {
                if (hoveredCard != null) hoveredCard.SetHighlight(false);
                hoveredCard = hitCard;
                if (hoveredCard != null) hoveredCard.SetHighlight(true);
            }

            SetLaserLength(hit.distance);

            if (OVRInput.GetDown(grabButton, controllerNode))
            {
                StartDragging(hoveredCard);
            }
        }
        else
        {
            if (hoveredCard != null)
            {
                hoveredCard.SetHighlight(false);
                hoveredCard = null;
            }
            SetLaserLength(maxDistance);
        }
    }

    void HandleDragging()
    {
        if (selectedObject == null) { isDragging = false; return; }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, canvasLayer))
        {
            // 1. 월드 좌표(hit.point)를 벽(hit.collider) 기준의 로컬 좌표로 변환
            Vector3 localHitPos = hit.collider.transform.InverseTransformPoint(hit.point);

            // 2. Z축 고정 (Z-Fighting 방지용)
            // 벽의 앞쪽으로 살짝 띄우기 위해 Z값을 고정합니다. (예: -0.02f 또는 0.02f)
            // Unity 2D나 UI는 보통 Z가 음수일 때 카메라 쪽으로 튀어나옵니다. (상황에 따라 부호 확인 필요)
            Vector3 targetLocalPos = new Vector3(localHitPos.x, localHitPos.y, GameManagerUX.Instance.intervalCardAndCanvas);

            // 3. 로컬 좌표로 이동 (localPosition 사용!)
            // 부드럽게 이동 (Lerp)
            selectedObject.transform.localPosition = Vector3.Lerp(
                selectedObject.transform.localPosition, 
                targetLocalPos, 
                Time.deltaTime * 20f
            );

            // 4. 회전은 벽에 딱 붙도록 초기화 (부모 회전을 그대로 따라가게)
            selectedObject.transform.localRotation = Quaternion.identity;

            // 레이저 길이 조절
            SetLaserLength(hit.distance);

            // =================================================================
            // [서버 전송] 이미 targetLocalPos가 로컬 좌표이므로 변환 없이 바로 전송 가능
            // =================================================================
            if (Time.time - lastSendTime > sendInterval)
            {
                if (NetworkManagerPython.Instance != null)
                {
                    NetworkManagerPython.Instance.SendCardMove(
                        selectedObject.cardID, 
                        new Vector2(targetLocalPos.x, targetLocalPos.y) // 계산된 로컬 좌표 그대로 전송
                    );
                }
                lastSendTime = Time.time;
            }
        }
        else
        {
            SetLaserLength(maxDistance);
        }

        if (OVRInput.GetUp(grabButton, controllerNode))
        {
            StopDragging();
        }
    }

    void SetLaserLength(float distance)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(1, new Vector3(0, 0, distance));
        }
    }

    void StartDragging(NetworkCard card)
    {
        selectedObject = card;
        isDragging = true;
        if (selectedObject != null) selectedObject.OnGrab();
    }

    void StopDragging()
    {
        if (selectedObject != null) selectedObject.OnRelease();
        selectedObject = null;
        isDragging = false;
        GameManagerUX.Instance.moveCount += 1;
    }
}