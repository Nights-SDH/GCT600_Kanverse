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
        if (pinch == null)
        {
            Debug.LogError("❌ BrushPose_OpenXR: PinchDetector component is NULL!");
            return;
        }

        if (pinch.indexTip == null || pinch.thumbTip == null || pinch.wrist == null)
        {
            Debug.LogError("❌ BrushPose_OpenXR: Hand joint references are NULL!");
            return;
        }

        if (!pinch.IsPinching)
        {
            brushModel.gameObject.SetActive(false);
            return;
        }

        // 핀치하면 붓 표시
        brushModel.gameObject.SetActive(true);

        // ===== 1) 손가락 위치 =====

        Vector3 thumb = pinch.thumbTip.position;
        Vector3 index = pinch.indexTip.position;

        // ===== 방향 계산 =====
        Vector3 forward = (index - thumb).normalized;  
        Vector3 palmUp = pinch.wrist.up;

        Vector3 right = Vector3.Cross(palmUp, forward).normalized;
        Vector3 up = Vector3.Cross(forward, right);

        Quaternion rot = Quaternion.LookRotation(forward, up);

        // 붓 방향 미세 조정(필요 시 수정)
        Quaternion finalRot = rot * Quaternion.Euler(0, 0, 200f);

        // ===== 위치 계산 =====
        Vector3 finalPos = pinch.PinchPosition + forward * 0.01f;

        // ===== 6) 적용 =====
        brushModel.position = finalPos;
        brushModel.rotation = finalRot;

        Debug.Log("indexTip: " + pinch.indexTip.position);
        Debug.Log("wrist: " + pinch.wrist.position);
        Debug.Log("forward(index→wrist): " + (pinch.indexTip.position - pinch.wrist.position).normalized);
        Debug.Log("wrist.forward: " + pinch.wrist.forward);
        Debug.Log("wrist.up: " + pinch.wrist.up);
        Debug.Log("brushPos: " + finalPos);

    }
}
