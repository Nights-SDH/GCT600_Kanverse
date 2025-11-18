using UnityEngine;

public class OpenXRHandPinchDetector : MonoBehaviour
{
    [Header("Hand Joint Transforms")]
    public Transform indexTip;
    public Transform thumbTip;
    public Transform wrist;

    [Header("Pinch Threshold")]
    public float pinchStartDistance = 0.008f;   // 핀치 시작 8mm
    public float pinchEndDistance   = 0.012f;   // 핀치 유지/종료 12mm

    public bool IsPinching { get; private set; }
    public Vector3 PinchPosition { get; private set; }

    private void Update()
    {
        if (indexTip == null || thumbTip == null)
        {
            IsPinching = false;
            return;
        }

        float dist = Vector3.Distance(indexTip.position, thumbTip.position);

        if (!IsPinching)
        {
            if (dist < pinchStartDistance)
                IsPinching = true;
        }
        // 핀치 유지 & 종료
        else
        {
            if (dist > pinchEndDistance)
                IsPinching = false;
        }

        PinchPosition = (indexTip.position + thumbTip.position) * 0.5f;
        
        Debug.Log("Pinching: " + IsPinching);
        Debug.Log("PinchPosition: " + PinchPosition);
    }

}
