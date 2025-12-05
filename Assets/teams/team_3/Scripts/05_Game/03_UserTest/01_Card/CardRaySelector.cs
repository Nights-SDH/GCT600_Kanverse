using UnityEngine;
using Oculus.Platform;

public class CardRaySelector : MonoBehaviour
{
    [Header("Ray Settings")]
    public float maxDistance = 3f;               // 레이 길이
    public LayerMask cardLayerMask;              // Card 레이어만 맞추기
    public LineRenderer lineRenderer;            // Inspector에서 할당

    [Header("Debug")]
    public Color normalColor = Color.white;
    public Color hitColor = Color.yellow;

    private Card currentHover;

    void Update()
    {
        // 1) Ray 계산
        Ray ray = new Ray(transform.position, transform.forward);

        // 2) LineRenderer로 눈에 보이게
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * maxDistance);
        }

        // 3) 카드에 Raycast
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, cardLayerMask))
        {
            // 카드 찾기 (Collider가 자식에 있어도 찾도록 GetComponentInParent)
            Card card = hit.collider.GetComponentInParent<Card>();

            if (card != currentHover)
            {
                // 이전 호버 카드 리셋
                if (currentHover != null)
                    SetCardHighlight(currentHover, false);

                currentHover = card;

                if (currentHover != null)
                    SetCardHighlight(currentHover, true);
            }

            // 레이 색상도 바꾸고 싶으면
            if (lineRenderer != null)
                lineRenderer.startColor = lineRenderer.endColor = hitColor;

            // TODO: 여기서 트리거 누르면 "선택" 로직 추가 가능
        }
        else
        {
            // 아무것도 안 맞으면
            if (currentHover != null)
            {
                SetCardHighlight(currentHover, false);
                currentHover = null;
            }

            if (lineRenderer != null)
                lineRenderer.startColor = lineRenderer.endColor = normalColor;
        }

        if (currentHover != null && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log("카드 선택됨: " + currentHover.name);
            // 여기서 currentHover에 대한 선택 로직 실행
        }
    }

    // 카드 하이라이트용 – 지금은 단순히 약간 키우기
    void SetCardHighlight(Card card, bool on)
    {
        if (card == null) return;

        Transform t = card.transform;
        t.localScale = on ? t.localScale * 1.05f : t.localScale / 1.05f;
    }
}
