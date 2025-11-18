using UnityEngine;
using XDPaint.Controllers.InputData.Base;

public class InputAdapterForXDPaint : BaseInputData
{
    public OpenXRHandPinchDetector pinch;
    public Transform brushTip;

    public override void OnUpdate()
    {
        if (brushTip == null) 
            return;

        Vector3 pos = brushTip.position;

        if (pinch != null && pinch.IsPinching)
        {
            base.OnPress(0, pos, 1f);
        }
        else
        {
            base.OnUp(0, pos);
        }
    }
}
