using UnityEngine;
using Photon.Pun;

public class NetworkCard : MonoBehaviourPun, IPunObservable
{
    // 카드의 원래 색상과 선택되었을 때의 색상
    private Renderer meshRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow;
    public PhotonTransformView photonTransformView;

    // 현재 로컬 플레이어가 이 카드를 잡고 있는지 여부
    public bool isInteracting = false;

    void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        if (meshRenderer != null) originalColor = meshRenderer.material.color;
    }

    void Update()
    {
        // 내가 잡고 움직일 때만 위치를 업데이트하고 네트워크로 전송
        if (photonView.IsMine && isInteracting)
        {
            // 위치 이동 로직은 Controller 스크립트에서 transform을 직접 제어함
        }
        if (meshRenderer != null)
        {
            meshRenderer.material.color = isInteracting ? highlightColor : originalColor;
        }
    }

    // --- 시각적 효과 (하이라이트) ---
    public void SetHighlight(bool active)
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = active ? highlightColor : originalColor;
        }
    }

    // --- 인터랙션 로직 ---
    public void OnGrab()
    {
        // Photon 소유권 가져오기 (이래야 내가 위치를 전송할 수 있음)
        photonView.RequestOwnership();
        isInteracting = true;
        SetHighlight(true); // 잡고 있는 동안도 하이라이트 유지
    }

    public void OnRelease()
    {
        isInteracting = false;
        SetHighlight(false);
    }

    // Photon Transform View 컴포넌트를 안 쓴다면 아래 코드로 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 내가 주인일 때 데이터 보냄
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(isInteracting);
        }
        else
        {
            // 남이 주인일 때 데이터 받음
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            this.isInteracting = (bool)stream.ReceiveNext();
        }
    }
}