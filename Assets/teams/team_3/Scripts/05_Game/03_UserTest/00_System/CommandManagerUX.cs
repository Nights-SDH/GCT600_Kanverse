using UnityEngine;

public class CommandManagerUX: MonoBehaviour
{
    public void Update()
    {
        SpawnCanvasOnWall.Instance.CheckUpdate();
        
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            NetworkManagerPython.Instance.RequestGameStart();
        }
    }
}