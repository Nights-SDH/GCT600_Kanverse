using UnityEngine;
using Meta.XR.MRUtilityKit; // 필수 네임스페이스

public class MRUKWallLayerSetter : MonoBehaviour
{
    [Header("Settings")]
    public string targetLayerName = "Wall"; // 변경할 레이어 이름

    void Start()
    {
        // MRUK가 준비되면 이벤트를 연결합니다.
        if (MRUK.Instance != null)
        {
            MRUK.Instance.RegisterSceneLoadedCallback(OnSceneLoaded);
        }
    }

    // 방 정보 로딩이 끝나면 호출되는 함수
    void OnSceneLoaded()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();

        if (room == null) return;

        // 방에 있는 모든 벽(WallAnchor)을 가져와서 레이어 변경
        foreach (var wallAnchor in room.WallAnchors)
        {
            // 벽 오브젝트와 그 자식들(Mesh, Collider 등)까지 모두 변경
            SetLayerRecursively(wallAnchor.gameObject, LayerMask.NameToLayer(targetLayerName));
        }

        Debug.Log($"[MRUK] 모든 벽의 Layer를 '{targetLayerName}'로 변경했습니다.");
    }

    // 자식 오브젝트까지 싹 다 바꾸는 재귀 함수
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (newLayer < 0) return; // 레이어가 없으면 중단

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}