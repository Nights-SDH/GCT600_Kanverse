using UnityEngine;
using XDPaint;                                 // PaintManager
using XDPaint.Controllers;                    // InputController
using XDPaint.Controllers.InputData.Base;     // BaseInputData

public class XRHandInputBridge : MonoBehaviour
{
    public Transform brushTip;                // BrushTip Transform
    public OpenXRHandPinchDetector pinch;     // Pinch Detector

    private BaseInputData inputData;

    void Start()
    {
        // PaintManager 가져오기
        PaintManager pm = FindObjectOfType<PaintManager>();
        if (pm == null)
        {
            Debug.LogError("❌ PaintManager not found in scene!");
            return;
        }

        // inputData private 필드를 Reflection으로 가져오기
        var field = typeof(PaintManager).GetField("inputData",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        inputData = field.GetValue(pm) as BaseInputData;

        if (inputData == null)
        {
            Debug.LogError("❌ Could not access PaintManager.inputData via reflection.");
        }
    }

    void Update()
    {
        if (brushTip == null || inputData == null)
            return;

        Vector3 pos = brushTip.position;

        // Hover
        inputData.OnHover(0, pos);

        // Press
        if (pinch.IsPinching)
        {
            inputData.OnPress(0, pos, 1f);
        }
        else
        {
            inputData.OnUp(0, pos);
        }
    }
}
