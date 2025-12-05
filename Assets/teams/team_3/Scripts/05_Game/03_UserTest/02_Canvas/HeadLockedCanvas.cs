using UnityEngine;

public class HeadLockedCanvas : MonoBehaviour
{
    public Transform cameraTransform;  // XR 카메라 (예: XR Origin 안의 Main Camera)
    public float distance = 1.5f;      // 시야 중심 앞에 표시할 거리 (미터 단위)

    void Start()
    {
        if (cameraTransform == null)
        {
            // XR Origin에 있는 카메라 자동 탐색
            Camera cam = Camera.main;
            if (cam != null)
                cameraTransform = cam.transform;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // 카메라 정면 앞 "distance"만큼 위치
        transform.position = cameraTransform.position + cameraTransform.forward * distance;

        // 항상 카메라를 바라보도록 회전
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}
