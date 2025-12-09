using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager: SingletonObject<TitleManager>
{
    public Button StartButton;
    public Button EndButton;

    void Start()
    {
        StartButton.onClick.AddListener(OnStartButtonClicked);
        EndButton.onClick.AddListener(OnEndButtonClicked);
    }

    public void OnStartButtonClicked()
    {
        SceneController.Instance.ChangeSceneWithLoading(SceneName.Lobby_LEDWall);
    }

    public void OnEndButtonClicked()
    {
        Application.Quit();
    }
}
