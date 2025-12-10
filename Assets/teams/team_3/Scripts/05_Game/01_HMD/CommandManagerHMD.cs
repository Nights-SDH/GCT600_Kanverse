using UnityEngine;

public class CommandManagerHMD: MonoBehaviour
{
    public OVRInput.Controller ControllerR = OVRInput.Controller.RTouch;
    public OVRInput.Controller ControllerL = OVRInput.Controller.LTouch;

    public void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, ControllerR))
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
            if (OVRInput.GetDown(OVRInput.Button.One, ControllerL))
            {
                if(SceneController.Instance.currentScene == SceneName.Lobby_LEDWall) LobbyManager.Instance.OnStartButtonClicked();
                ConnectionManager.Instance.SendGameStart();
            }
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, ControllerR))
            {
                ConnectionManager.Instance.SendNextScenario();
            }
        }
    }
}