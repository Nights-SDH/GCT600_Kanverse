using UnityEngine;

public class CommandManagerUX: MonoBehaviour
{
   
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SpawnCanvasOnWall.Instance.SpawnCanvas();
        }
    }
}