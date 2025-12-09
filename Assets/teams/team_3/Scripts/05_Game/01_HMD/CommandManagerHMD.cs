using Unity.VisualScripting;
using UnityEngine;

public class CommandManagerHMD: MonoBehaviour
{
    public OVRInput.Controller ControllerR = OVRInput.Controller.RTouch;
    public OVRInput.Controller ControllerL = OVRInput.Controller.LTouch;

    public void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, ControllerR) || Input.GetKeyDown(KeyCode.Space))
        {
            if(RatioAlignedCanvas.InstanceWithoutCreate == null)
            {
                SpawnCanvasOnWall.Instance.TrySpawnCanvas();
            } else
            {
                RatioAlignedCanvas.Instance.gameObject.SetActive(!RatioAlignedCanvas.Instance.gameObject.activeSelf);
            }
        }
        if(ConnectionManager.Instance.IsHost)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two, ControllerL) || Input.GetKeyDown(KeyCode.N))
            {
                ConnectionManager.Instance.SendGameStart();
            }
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, ControllerR) || Input.GetKeyDown(KeyCode.N))
            {
                ConnectionManager.Instance.SendNextScenario();
            }
        }
    }
}