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
    private int count = 0;

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
            // 1. 시각적 이동 (World Space)
            Vector3 targetPos = hit.point + (hit.normal * 0.02f);
            
            // 부드러운 이동
            selectedObject.transform.position = Vector3.Lerp(selectedObject.transform.position, new Vector3(targetPos.x, targetPos.y, selectedObject.transform.position.z), Time.deltaTime * 20f);

            // 레이저 길이 조절
            SetLaserLength(hit.distance);

            // =================================================================
            // [추가됨] 2. 서버로 좌표 전송 로직
            // =================================================================
            if (Time.time - lastSendTime > sendInterval)
            {
                // (중요) 월드 좌표(hit.point)를 그대로 보내면 상대방 방 위치가 다를 때 문제 생김.
                // 따라서 '닿은 캔버스(벽)' 기준의 로컬 좌표로 변환해서 보냄.
                Vector3 localPos = hit.collider.transform.InverseTransformPoint(targetPos);

                if (NetworkManagerPython.Instance != null)
                {
                    // 로컬 X, Y 좌표 전송
                    NetworkManagerPython.Instance.SendCardMove(
                        selectedObject.cardID, 
                        new Vector2(localPos.x, localPos.y)
                    );
                }

                lastSendTime = Time.time;
            }
            // =================================================================
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
        count+=1;
        Debug.Log($"카드 드래그 {count}");
    }
}