using UnityEngine;

public class UserFollowUI : MonoBehaviour
{
    [Tooltip("카메라와 UI 사이의 고정 거리")]
    public float followDistance = 2.0f;

    [Tooltip("위치 이동의 부드러움 (높을수록 빠름)")]
    public float positionSmoothSpeed = 8.0f;

    [Tooltip("회전의 부드러움 (높을수록 빠름)")]
    public float rotationSmoothSpeed = 5.0f;

    // 카메라 움직임이 모두 끝난 후 실행되어 떨림(Jitter) 현상을 방지
    void LateUpdate()
    {
        if (UserCamera.InstanceWithoutCreate == null)
        {
            Debug.LogWarning("SmoothFollowUI: Camera Target이 설정되지 않았습니다.");
            return;
        }

        // --- 1. 위치(Position) 계산 ---
        
        // 카메라의 '앞' 방향으로 followDistance만큼 떨어진 곳을 목표 위치로 정합니다.
        Vector3 targetPosition = UserCamera.Instance.transform.position + UserCamera.Instance.transform.forward * followDistance;

        // Vector3.Lerp를 사용해 현재 위치에서 목표 위치로 부드럽게 이동시킵니다.
        transform.position = Vector3.Lerp(
            transform.position, 
            targetPosition, 
            positionSmoothSpeed * Time.deltaTime
        );


        // --- 2. 회전(Rotation) 계산 ---

        // UI가 항상 카메라를 정면으로 바라보도록(Billboard) 목표 회전값을 계산합니다.
        // Quaternion.LookRotation(A, B)
        // A: 바라볼 방향 (UI 위치 -> 카메라 위치)
        // B: 위쪽 방향 (카메라의 위쪽 방향과 일치시켜 UI가 뒤집히지 않게 함)
        Quaternion targetRotation = Quaternion.LookRotation(
            transform.position - UserCamera.Instance.transform.position, // UI에서 카메라를 바라보는 벡터
            UserCamera.Instance.transform.up                           // 카메라의 '위' 방향
        );

        // Quaternion.Slerp를 사용해 현재 회전에서 목표 회전으로 부드럽게 회전시킵니다.
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            rotationSmoothSpeed * Time.deltaTime
        );
    }
}
