using UnityEngine;

public class NetworkCard : MonoBehaviour
{
    public int cardID;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    
    [Header("Settings")]
    public Color highlightColor = Color.yellow;
    public Color lockedColor = Color.gray; // [추가] 다른 사람이 잡았을 때 색상

    // 상태 변수
    public bool isInteracting = false; // 내가 잡고 있는가?
    public bool isLocked = false;      // 남이 잡고 있는가?

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // 내가 잡고 움직일 때의 로직 (필요 시 작성)
        if (isInteracting)
        {
            // 위치 이동 등은 Controller(Interactor)에서 처리
        }
    }

    public void SetCardID(int id)
    {
        cardID = id;
    }

    // --- [로컬] 인터랙션 로직 (내가 잡을 때) ---
    public void OnGrab()
    {
        if (isLocked) return; // 이미 잠긴 카드는 잡을 수 없음

        isInteracting = true;
        SetHighlight(true);

        // [추가] 서버에 "나 이거 잡았어" 알림
        if (NetworkManagerPython.Instance != null)
        {
            NetworkManagerPython.Instance.SendCardGrab(cardID);
        }
    }

    public void OnRelease()
    {
        isInteracting = false;
        SetHighlight(false);

        // [추가] 서버에 "나 이거 놨어" 알림
        if (NetworkManagerPython.InstanceWithoutCreate != null)
        {
            NetworkManagerPython.Instance.SendCardRelease(cardID);
        }
    }

    // --- [리모트] 잠금 처리 (남이 잡았을 때 NetworkManager가 호출) ---
    public void SetRemoteLock(bool locked)
    {
        isLocked = locked;

        if (spriteRenderer != null)
        {
            if (isLocked)
            {
                // 잠김 상태: 회색으로 변경 (하이라이트보다 우선순위 높음)
                spriteRenderer.color = lockedColor;
            }
            else
            {
                // 잠김 해제: 원래 색으로 복귀
                spriteRenderer.color = originalColor;
            }
        }
    }

    // --- 시각적 효과 (하이라이트) ---
    public void SetHighlight(bool active)
    {
        // [중요] 잠금 상태(남이 잡음)이거나 내가 이미 잡고 있으면 하이라이트 변경 무시
        if (isLocked) return; 

        if (spriteRenderer != null)
        {
            spriteRenderer.color = active ? highlightColor : originalColor;
        }
    }
}