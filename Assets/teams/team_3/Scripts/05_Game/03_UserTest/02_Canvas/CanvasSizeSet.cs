using UnityEngine;

public enum CanvasSizeType
{
    Tutorial,
    Baseline,
    Absolute,
    Wide1,
    Wide2,
    Wide3,
    Tall1,
    Tall2,
    Tall3,
}

[System.Serializable]
public class CanvasSizeSet
{
    public CanvasSizeType type;
    public float widthScale;
    public float heightScale;
}