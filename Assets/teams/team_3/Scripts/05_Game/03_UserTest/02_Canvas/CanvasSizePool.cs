using UnityEngine;

public class CanvasSizePool: SingletonObject<CanvasSizePool>
{
    public CanvasSizeSet[] canvasSizes;

    public CanvasSizeSet GetCurrentCanvasSizeSet()
    {
        GameManagerUX gm = GameManagerUX.Instance;
        foreach (var sizeSet in canvasSizes)
        {
            if (sizeSet.type == gm.selectedCanvasSizeType)
            {
                return sizeSet;
            }
        }
        Debug.LogWarning($"선택된 CanvasSizeType {gm.selectedCanvasSizeType}에 해당하는 CanvasSizeSet이 없습니다.");
        return null;
    }
}
