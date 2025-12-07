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
    // rayOrigin이 없으면 이 스크립트가 붙은 오브젝트(LaserBeam)가 기준이 됨
    
    private NetworkCard hoveredCard = null;
    private NetworkCard selectedObject = null;
    private bool isDragging = false;

    void Start()
    {
        // 로컬 좌표계 사용 강제 설정
        if (lineRenderer != null)
        {
            lineRenderer.useWorldSpace = false; // 핵심!
            lineRenderer.SetPosition(0, Vector3.zero); // 시작점은 항상 (0,0,0)
        }
    }

    void Update()
    {
        // 이제 transform.position/forward는 LineRenderer가 붙은 자식 오브젝트 기준
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
            Debug.Log("[SDH]" + hit.collider.gameObject.name);
            NetworkCard hitCard = hit.collider.GetComponent<NetworkCard>();

            if (hoveredCard != hitCard)
            {
                if (hoveredCard != null) hoveredCard.SetHighlight(false);
                hoveredCard = hitCard;
                if (hoveredCard != null) hoveredCard.SetHighlight(true);
            }

            // [변경점] 좌표 대신 '거리'만 넘겨줍니다.
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
            // 허공이면 최대 길이
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
            Debug.Log("[SDH] canvasLayer detected" + hit.collider.gameObject.name);
            Vector3 targetPos = hit.point + (hit.normal * 0.02f);
            Quaternion targetRot = Quaternion.LookRotation(hit.normal);

            selectedObject.transform.position = Vector3.Lerp(selectedObject.transform.position, targetPos, Time.deltaTime * 20f);
            selectedObject.transform.rotation = Quaternion.Slerp(selectedObject.transform.rotation, targetRot, Time.deltaTime * 20f);

            // 벽까지의 거리로 길이 조절
            SetLaserLength(hit.distance);
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

    // [핵심 변경] 시작점/끝점 좌표 계산 없이 길이(Z)만 조절
    void SetLaserLength(float distance)
    {
        if (lineRenderer != null)
        {
            // Index 1번(끝점)의 Z좌표만 변경하면 로컬 좌표계라 알아서 방향 맞춤
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
    }
}