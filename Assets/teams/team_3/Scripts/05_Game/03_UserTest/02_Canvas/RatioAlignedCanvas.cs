using UnityEngine;
using Meta.XR.MRUtilityKit;

/// <summary>
/// 방 크기를 읽어서 캔버스를 정규화된 비율로 스케일링.
/// WallRaycastRight가 Ratio 모드일 때만 활성화됨.
/// </summary>
public class RatioAlignedCanvas : SingletonObject<RatioAlignedCanvas>
{
    private const float StandardLength = 100;

    [Tooltip("CanvasArea(면적)")]
    public float canvasArea = 120000f;
    private float canvasAreaCache;
    public void SetCanvasSize(float area)
    {
        canvasArea = area;
        canvasAreaCache = area;
    }

    [Tooltip("widthScale(가로비)")] // 비율 조정용
    public float xScale = 4f;
    private float xScaleCache;
    public void SetXScale(float xScale)
    {
        this.xScale = xScale;
        xScaleCache = xScale;
    }

    [Tooltip("heightScale(세로비)")] // 비율 조정용
    public float yScale = 3f;
    private float yScaleCache;
    public void SetYScale(float yScale)
    {
        this.yScale = yScale;
        yScaleCache = yScale;
    }


    [Tooltip("width(가로 길이)")] // 비율 조정용
    public float xLength = 40f;
    private float xLengthCache;
    public void SetXLength(float xLength)
    {
        this.xLength = xLength;
        xLengthCache = xLength;
        gameObject.transform.localScale = new Vector3(xLength / StandardLength, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    [Tooltip("height(세로 길이)")] // 비율 조정용
    public float yLength = 30f;
    private float yLengthCache;
    public void SetYLength(float yLength)
    {
        this.yLength = yLength;
        yLengthCache = yLength;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, yLength / StandardLength, gameObject.transform.localScale.z);
    }

    public void OnValidate()
    {
        CheckChange();
    }

    public void Update()
    {
        CheckChange();
    }

    public void CheckChange()
    {
        if(CheckChangeCanvasArea()) return;
        if(CheckChangeScale()) return;
        if(CheckChangeCanvasLength()) return;
    }

    private bool CheckChangeCanvasArea()
    {
        if (canvasAreaCache == canvasArea) return false;

        float r = Mathf.Sqrt(canvasArea / (xScale * yScale));
        SetXLength(r * xScale);
        SetYLength(r * yScale);
        return true;
    }

    private bool CheckChangeScale() 
    {
        if (xScaleCache == xScale && yScaleCache == yScale) return false;
        
        float r = Mathf.Sqrt(canvasArea / (xScale * yScale));
        SetXLength(r * xScale);
        SetYLength(r * yScale);
        SetXScale(xScale);
        SetYScale(yScale);
        return true;
    }

    private bool CheckChangeCanvasLength()
    {
        if(xLengthCache == xLength && yLengthCache == yLength) return false;
        if (xLengthCache != xLength)
        {
            SetXLength(xLength);
            SetYLength(canvasArea/xLength);
            SetXScale(xLength/yLength);
            SetYScale(1);
        }
        else if (yLengthCache != yLength)
        {
            SetYLength(yLength);
            SetXLength(canvasArea/yLength);
            SetYScale(yLength/xLength);
            SetXScale(1);
        }
        return true;
    }
}
