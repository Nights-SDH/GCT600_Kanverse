using UnityEngine;
using Photon.Pun;

public class MRUKLaserInteractor : MonoBehaviour
{
    [Header("Settings")]
    public float maxDistance = 10f;
    public OVRInput.Button commandButton = OVRInput.Button.One; // A버튼 혹은 X버튼
    public LayerMask cardLayer; // "Card" 레이어만 체크
    public LayerMask canvasLayer; // "Wall/Canvas" 레이어만 체크

    [Header("References")]
    public LineRenderer lineRenderer;

    // 내부 상태 변수
    private NetworkCard hoveredCard = null; // 현재 바라보고 있는 카드
    private NetworkCard selectedObject = null; // 현재 잡고 있는 카드
    private bool isDragging = false;

    void Update()
    {
        // 드래그 중이냐 아니냐에 따라 로직 분기
        if (isDragging)
        {
            HandleDragging();
        }
        else
        {
            HandleHovering();
        }
    }

    // 1. 카드를 안 잡고 있을 때: 탐색 및 하이라이트
    void HandleHovering()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Card 레이어만 검사
        if (Physics.Raycast(ray, out hit, maxDistance, cardLayer))
        {
            NetworkCard hitCard = hit.collider.GetComponent<NetworkCard>();

            // 새로운 카드를 바라봄
            if (hoveredCard != hitCard)
            {
                if (hoveredCard != null) hoveredCard.SetHighlight(false); // 이전 카드 끄기
                hoveredCard = hitCard;
                if (hoveredCard != null) hoveredCard.SetHighlight(true); // 새 카드 켜기
            }

            DrawLaser(hit.point);

            // Command 버튼 누르면 -> 드래그 시작
            if (OVRInput.GetDown(commandButton, OVRInput.Controller.RTouch)) // 혹은 LTouch
            {
                StartDragging(hoveredCard);
            }
        }
        else
        {
            // 허공을 바라봄
            if (hoveredCard != null)
            {
                hoveredCard.SetHighlight(false);
                hoveredCard = null;
            }
            DrawLaser(transform.position + transform.forward * maxDistance);
        }
    }

    // 2. 카드를 잡았을 때: Canvas 위에서 이동
    void HandleDragging()
    {
        if (selectedObject == null)
        {
            isDragging = false;
            return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // 중요: 드래그 중에는 'Card'는 무시하고 뒤에 있는 'Canvas(Wall)'만 찾아서 좌표를 구함
        if (Physics.Raycast(ray, out hit, maxDistance, canvasLayer))
        {
            // 카드를 Canvas 표면(hit.point)으로 이동
            // Z-fighting 방지를 위해 법선 방향으로 아주 살짝 띄움 (+0.01f)
            Vector3 targetPos = hit.point + (hit.normal * 0.02f);
            Quaternion targetRot = Quaternion.LookRotation(hit.normal); // 벽을 보게 회전

            // 부드러운 이동 (선택사항)
            selectedObject.transform.position = Vector3.Lerp(selectedObject.transform.position, targetPos, Time.deltaTime * 15f);
            selectedObject.transform.rotation = Quaternion.Slerp(selectedObject.transform.rotation, targetRot, Time.deltaTime * 15f);

            DrawLaser(hit.point);
        }
        else
        {
            // 캔버스를 벗어나면? 그냥 레이저만 김
             DrawLaser(transform.position + transform.forward * maxDistance);
        }

        // Command 버튼 다시 누르면 -> 드래그 해제 (고정)
        if (OVRInput.GetDown(commandButton, OVRInput.Controller.RTouch))
        {
            StopDragging();
        }
    }

    void StartDragging(NetworkCard card)
    {
        selectedObject = card;
        isDragging = true;
        
        // Photon 소유권 및 상태 변경 요청
        if (selectedObject != null)
        {
            selectedObject.OnGrab();
        }
    }

    void StopDragging()
    {
        if (selectedObject != null)
        {
            selectedObject.OnRelease();
        }
        selectedObject = null;
        isDragging = false;
    }

    void DrawLaser(Vector3 endPos)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, endPos);
        }
    }
}