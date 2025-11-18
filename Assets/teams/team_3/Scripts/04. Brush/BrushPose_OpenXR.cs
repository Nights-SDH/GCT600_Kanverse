using UnityEngine;

public class BrushPose_OpenXR : MonoBehaviour
{
    public OpenXRHandPinchDetector pinch;
    public Transform brushModel;
    public Transform brushTip;

    [Header("Offsets")]
    public float gripDistance = 0.015f;

    private void Update()
    {
        if (!pinch.IsPinching)
        {
            brushModel.gameObject.SetActive(false);
            return;
        }

        brushModel.gameObject.SetActive(true);

        // ===== 1) 손가락 위치 =====
        Vector3 thumb = pinch.thumbTip.position;
        Vector3 index = pinch.indexTip.position;
        Vector3 wrist = pinch.wrist.position;

        // ===== 2) 붓이 나아갈 방향(정방향) =====
        Vector3 forward = (index - thumb).normalized;  

        // ===== 3) 손바닥 방향 (up) =====
        Vector3 palmUp = pinch.wrist.up;  

        // ===== 4) forward 와 palmUp 이 직교하도록 재계산 =====
        Vector3 right = Vector3.Cross(palmUp, forward).normalized;
        Vector3 up = Vector3.Cross(forward, right);

        Quaternion rot = Quaternion.LookRotation(forward, up);

        // ===== 5) 위치 계산: pinch point 기준 =====
        Vector3 finalPos = pinch.PinchPosition + forward * 0.01f;

        // ===== 6) 적용 =====
        brushModel.position = finalPos;
        brushModel.rotation = rot;

        Debug.Log("indexTip: " + pinch.indexTip.position);
        Debug.Log("wrist: " + pinch.wrist.position);
        Debug.Log("forward(index→wrist): " + (pinch.indexTip.position - pinch.wrist.position).normalized);
        Debug.Log("wrist.forward: " + pinch.wrist.forward);
        Debug.Log("wrist.up: " + pinch.wrist.up);
        Debug.Log("brushPos: " + finalPos);

    }
}
