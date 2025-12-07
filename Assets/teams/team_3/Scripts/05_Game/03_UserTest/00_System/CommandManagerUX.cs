using UnityEngine;

public class CommandManagerUX: MonoBehaviour
{
   
    public void Update()
    {
        SpawnCanvasOnWall.Instance.CheckUpdate();
    }
}