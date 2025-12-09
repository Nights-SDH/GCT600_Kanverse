using UnityEngine;

public class CommandManagerHMD: MonoBehaviour
{
    public OVRInput.Controller ControllerR = OVRInput.Controller.RTouch;
    public OVRInput.Controller ControllerL = OVRInput.Controller.LTouch;

    public void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, ControllerR) || Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCanvasOnWall.Instance.TrySpawnCanvas();
        }
        if (OVRInput.GetDown(OVRInput.Button.One, ControllerL))
        {
            NetworkManagerPython.Instance.RequestGameStart();
        }
    }
}