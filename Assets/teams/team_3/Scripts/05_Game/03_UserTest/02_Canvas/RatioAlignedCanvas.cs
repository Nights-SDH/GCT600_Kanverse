using UnityEngine;
using Meta.XR.MRUtilityKit;

/// <summary>
/// 방 크기를 읽어서 캔버스를 정규화된 비율로 스케일링.
/// WallRaycastRight가 Ratio 모드일 때만 활성화됨.
/// </summary>
public class RatioAlignedCanvas : MonoBehaviour
{
    [Tooltip("referenceRoomSize(m)")] // 기준 방 크기 (예: 5미터 방)
    public float referenceRoomSize = 5f;

    [Tooltip("scale")] // 비율 조정용
    public float roomScalingFactor = 1.0f;

    [Tooltip("Scale Canvas Root")] // 비율 조정할 캔버스 오브젝트
    public GameObject canvasObject;

    void Start()
    {
        if (canvasObject == null)
            canvasObject = this.gameObject;  // Prefab Root 사용
    }

    void Update()
    {
        if (MRUK.Instance == null) return;

        var room = MRUK.Instance.GetCurrentRoom();
        if (room != null)
            ApplyRatioAlignment(room);
    }

    void ApplyRatioAlignment(MRUKRoom room)
    {
        if (canvasObject == null) return;

        Bounds roomBounds = room.GetRoomBounds();
        Vector3 roomSize = roomBounds.size;

        float ratioX = (roomSize.x / referenceRoomSize) * roomScalingFactor;
        float ratioY = (roomSize.y / referenceRoomSize) * roomScalingFactor;
        float ratioZ = (roomSize.z / referenceRoomSize) * roomScalingFactor;

        Vector3 newScale = new Vector3(ratioX, ratioY, ratioZ);

        canvasObject.transform.localScale = newScale;

        Debug.Log(
            $"[RatioAligned] Room Size = {roomSize}, Ratio = {newScale}, Applied to {canvasObject.name}"
        );
    }
}
