using UnityEngine;

/// <summary>
/// 절대 정합 모드: Canvas는 스케일을 변경하지 않고
/// WallRaycastRight에서 설정한 P1/P2/P3 크기를 그대로 사용.
/// 필요하면 추가적인 위치/회전 로직을 넣을 수 있음.
/// </summary>
public class AbsoluteCanvasController : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[AbsoluteAlign] Canvas activated (no scale modification).");
    }

    void Update()
    {
        // 절대 정합은 스케일을 건드리지 않음.
        // 필요하면 여기서 위치/회전의 안정화 또는 smoothing 가능.
    }
}
