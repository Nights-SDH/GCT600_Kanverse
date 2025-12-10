using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager: SingletonObject<LobbyManager>
{
    private const int MAX_PARTICIPANTS = 4;
    public TMP_Text roomInfoViewer;
    public List<GameObject> participantsList;
    public Button startButton;

    private int participantCount = 0;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        ConnectionManager.Instance.SendGetRoomInfo();
    }
    public void AddParticipant()
    {
        if(participantCount == MAX_PARTICIPANTS) return;

        participantCount++;
        UpdateRoomInfo(participantCount);
    }

    public void UpdateRoomInfo(int count)
    {
        if(count != participantCount)
        {
            participantCount = count;
            for(int i = 0; i < participantCount; i++)
            {
                participantsList[i].SetActive(true);
            }
            for (int i = participantCount; i < MAX_PARTICIPANTS; i++)
            {
                participantsList[i].SetActive(false);
            }
            roomInfoViewer.text = $"RoomId: 1004 ({participantCount}/{MAX_PARTICIPANTS})";   
        }
    }

    public void OnStartButtonClicked()
    {
        StartCoroutine(SceneController.Instance.ChangeSceneWithLoading(SceneName.InGame_LEDWall));
    }
}
