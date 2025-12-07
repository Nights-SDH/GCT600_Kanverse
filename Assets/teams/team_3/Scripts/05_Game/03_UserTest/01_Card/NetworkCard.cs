using UnityEngine;
using Photon.Pun;

public class NetworkCard : MonoBehaviourPun, IPunObservable
{
    // [변경 1] SpriteRenderer로 변경
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow;
    
    // PhotonTransformView를 쓴다면 Inspector에서 체크하고 여기선 변수 선언 안 해도 됨 (옵션)
    // public PhotonTransformView photonTransformView; 

    public bool isInteracting = false;

    void Awake()
    {
        // [변경 2] 컴포넌트 가져오기 수정
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // SpriteRenderer의 color 프로퍼티 사용
        if (spriteRenderer != null) originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // 내가 잡고 움직일 때만 로직 처리
        if (photonView.IsMine && isInteracting)
        {
            // 위치 이동 로직은 Controller(LaserInteractor)가 처리함
        }

        // [중요] Update()에 있던 색상 변경 코드는 삭제했습니다.
        // 이유: 매 프레임 색을 원래대로 돌리려는 성질 때문에
        // 레이저가 닿았을 때(Hover) 색이 변하지 않거나 깜빡거리는 문제를 방지하기 위함입니다.
    }

    // --- 시각적 효과 (하이라이트) ---
    public void SetHighlight(bool active)
    {
        if (spriteRenderer != null)
        {
            // [변경 3] Material 대신 Sprite 자체 Color 변경 (성능상 더 좋음)
            spriteRenderer.color = active ? highlightColor : originalColor;
        }
    }

    // --- 인터랙션 로직 ---
    public void OnGrab()
    {
        photonView.RequestOwnership();
        isInteracting = true;
        SetHighlight(true);
    }

    public void OnRelease()
    {
        isInteracting = false;
        SetHighlight(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(isInteracting);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            
            bool previousState = this.isInteracting;
            this.isInteracting = (bool)stream.ReceiveNext();

            // [추가] 다른 플레이어가 잡았을 때 내 화면에서도 색이 변하게 동기화
            if (previousState != this.isInteracting)
            {
                SetHighlight(this.isInteracting);
            }
        }
    }
}