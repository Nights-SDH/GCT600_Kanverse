using TMPro;
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

    public void ChangeButtonName()
    {
        StartButton.GetComponentInChildren<TMP_Text>().text = "방 입장";
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("Start Button Clicked");
        StartCoroutine(SceneController.Instance.ChangeSceneWithLoading(SceneName.Lobby_LEDWall));
    }

    public void OnEndButtonClicked()
    {
        Application.Quit();
    }
}
