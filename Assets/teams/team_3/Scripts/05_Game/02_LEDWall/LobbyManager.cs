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

        participantsList[participantCount].SetActive(true); 
        participantCount++;
        roomInfoViewer.text = $"RoomId: 1004 ({participantCount}/{MAX_PARTICIPANTS})";   
    }

    public void UpdateRoomInfo(int count)
    {
        if(count != participantCount)
        {
            for(int i = participantCount; i < count; i++)
            {
                participantsList[i].SetActive(true);
            }
            participantCount = count;
            roomInfoViewer.text = $"RoomId: 1004 ({participantCount}/{MAX_PARTICIPANTS})";   
        }
    }

    public void OnStartButtonClicked()
    {
        StartCoroutine(SceneController.Instance.ChangeSceneWithLoading(SceneName.InGame_LEDWall));
    }
}
