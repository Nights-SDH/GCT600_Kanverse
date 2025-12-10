using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Concurrent;


[Serializable]
public class SocketMessageFinal
{
    public string type;
    public string device_type; // REGISTER용
    public string role;        // ROLE_ASSIGN용
    public int playerCount;    // ROOM_UPDATE용
    public bool isRoomCreated; // ROOM_UPDATE용
    public int object_info;
}

public class ConnectionManager : SingletonObject<ConnectionManager>
{
    [Header("Network Settings")]
    
    [Header("Device Configuration")]
    public DeviceType myDeviceType = DeviceType.HMD; // Inspector에서 설정

    private ClientWebSocket ws = new ClientWebSocket();
    private CancellationTokenSource cts = new CancellationTokenSource();
    private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();

    public bool IsHost { get; private set; } = false;
    public bool IsConnected => ws != null && ws.State == WebSocketState.Open;

    private async void Start()
    {
        await ConnectToServer();
    }

    private void OnDestroy()
    {
        cts.Cancel();
        if (ws != null) ws.Dispose();
    }

    private async Task ConnectToServer()
    {
        try
        {
            ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri(NetworkFunctionsProject.serverUrl), cts.Token);
            Debug.Log("[Net] 서버 연결됨. 등록 절차 진행...");

            _ = ReceiveLoop();

            // [중요] 연결 직후 내 정체(DeviceType)를 서버에 등록
            SendRegister();
        }
        catch (Exception e)
        {
            Debug.LogError($"[Net] 연결 실패: {e.Message}");
        }
    }

    // --- 송신 패킷들 ---

    // 1. 등록 (접속 시 자동 호출)
    public void SendRegister()
    {
        SocketMessageFinal msg = new SocketMessageFinal
        {
            type = "REGISTER",
            device_type = myDeviceType.ToString() // "HMD" or "LED_WALL"
        };
        SendJson(msg);
    }

    // 2. 게임 시작 (Host만 호출)
    public void SendGameStart()
    {
        if (myDeviceType == DeviceType.HMD && IsHost)
        {
            SendJson(new SocketMessageFinal { type = "START_GAME" });
        }
    }

    // 3. 로딩 완료 (LED Wall만 호출 - 씬 로드 끝난 후 호출하세요)
    public void SendLoadingComplete()
    {
        if (myDeviceType == DeviceType.LED_WALL)
        {
            SendJson(new SocketMessageFinal { type = "LOADING_COMPLETE" });
        }
    }

    public void SendGetRoomInfo()
    {
        if (myDeviceType == DeviceType.LED_WALL)
        {
            SendJson(new SocketMessageFinal { type = "GET_ROOM_INFO" });
        }
    }

    // 4. 다음 시나리오 (Host만 호출)
    public void SendNextScenario()
    {
        if (myDeviceType == DeviceType.HMD && IsHost)
        {
            SendJson(new SocketMessageFinal { type = "NEXT_SCENARIO" });
        }
    }

    public void SendSpawn3DInfo(DialogSpeaker dialogSpeaker)
    {
        if (myDeviceType == DeviceType.HMD && IsHost)
        {
            SendJson(new SocketMessageFinal { type = "Spawn_3D_Object" , object_info = (int)dialogSpeaker});
        }
    }

    // --- 수신 루프 및 처리 ---

    private async Task ReceiveLoop()
    {
        var buffer = new byte[1024 * 4];
        while (ws.State == WebSocketState.Open && !cts.IsCancellationRequested)
        {
            try
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
                if (result.MessageType == WebSocketMessageType.Close) break;

                string json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                messageQueue.Enqueue(json);
            }
            catch { break; }
        }
    }

    private void Update()
    {
        while (messageQueue.TryDequeue(out string json))
        {
            Debug.Log($"[Net] 수신된 메시지: {json}");
            ProcessMessage(json);
        }
    }

    private void ProcessMessage(string json)
    {
        SocketMessageFinal msg = JsonUtility.FromJson<SocketMessageFinal>(json);

        switch (msg.type)
        {
            case "ROLE_ASSIGN":
                IsHost = (msg.role == "HOST");
                Debug.Log($"[Net] [내 역할] {msg.role}");
                break;

            case "ROOM_INFO":
                // LED Wall이 받는 정보 (현재 인원수 등)
                Debug.Log($"[Net] [LED Wall Info] Player Count: {msg.playerCount}");
                if(myDeviceType == DeviceType.LED_WALL)
                {
                    if(SceneController.Instance.currentScene == SceneName.Lobby_LEDWall)
                    {
                        LobbyManager.Instance.UpdateRoomInfo(msg.playerCount);
                    }
                    
                }
                break;

            case "ROOM_UPDATE":
                // LED Wall이 받는 정보 (현재 인원수 등)
                Debug.Log($"[Net] [LED Wall Info] Player Count: {msg.playerCount}, Room Created: {msg.isRoomCreated}");
                if(myDeviceType == DeviceType.LED_WALL)
                {
                    if(msg.isRoomCreated && SceneController.Instance.currentScene == SceneName.Title_LEDWall && IsHost == false)
                    {
                        TitleManager.Instance.OnStartButtonClicked();
                    } else if(SceneController.Instance.currentScene == SceneName.Lobby_LEDWall)
                    {
                        LobbyManager.Instance.UpdateRoomInfo(msg.playerCount);
                    }
                    
                }
                break;

            case "GAME_START":
                Debug.Log("[Net] 게임 시작! 씬 로딩을 시작합니다...");
                if(myDeviceType == DeviceType.LED_WALL && SceneController.Instance.currentScene == SceneName.Lobby_LEDWall)
                {
                    LobbyManager.Instance.OnStartButtonClicked();
                }
                break;

            case "SCENARIO_START":
                Debug.Log("[Net] 모든 LED Wall 로딩 완료. 시나리오 시작!");
                if(myDeviceType == DeviceType.LED_WALL && SceneController.Instance.currentScene == SceneName.InGame_LEDWall)
                {
                    DialogManager.Instance.StartDialog(DialogName.Scene1_Intro);
                }
                break;

            case "NEXT_SCENARIO":
                Debug.Log("[Net] 다음 시나리오를 재생합니다.");
                if(myDeviceType == DeviceType.LED_WALL && SceneController.Instance.currentScene == SceneName.InGame_LEDWall)
                {
                    DialogManager.Instance.CommandCheck();
                }
                break;

            case "Spawn_3D_Object":
                Debug.Log($"[Net] 3D 오브젝트 {msg.object_info} 생성 명령 수신.");
                if(myDeviceType == DeviceType.HMD && SceneController.Instance.currentScene == SceneName.HMD_InGame)
                {
                    VRHeadSpawner.Instance.SpawnNextOnFloor((ObjectName)msg.object_info);
                }
                break;
        }
    }

    private async void SendJson(SocketMessageFinal msg)
    {
        if (ws.State != WebSocketState.Open) return;
        string json = JsonUtility.ToJson(msg);
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cts.Token);
    }
}