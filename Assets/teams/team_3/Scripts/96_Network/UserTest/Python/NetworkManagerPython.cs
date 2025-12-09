using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerPython : SingletonObject<NetworkManagerPython>
{
    [SerializeField] private string serverUrl = "wss://arpserver-production.up.railway.app/ws";

    // 상태 변수
    private ClientWebSocket ws = new ClientWebSocket();
    private CancellationTokenSource cts = new CancellationTokenSource();
    private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();

    public bool IsHost => GameManagerUX.Instance.isHost;
    public bool IsConnected => ws.State == WebSocketState.Open;

    private async void Start()
    {
        await ConnectToServer();
    }

    private void OnDestroy()
    {
        cts.Cancel();
        if (ws != null && ws.State == WebSocketState.Open)
            ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
    }

    // --- 1. 서버 연결 ---
    private async Task ConnectToServer()
    {
        try
        {
            ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri(serverUrl), cts.Token);
            Debug.Log("[SDH] 서버에 연결되었습니다.");

            _ = ReceiveLoop();
        }
        catch (Exception e)
        {
            Debug.LogError($"[SDH] 연결 실패: {e.Message}");
        }
    }

    // --- 2. 메시지 수신 (백그라운드) ---
    private async Task ReceiveLoop()
    {
        var buffer = new byte[1024 * 4];

        while (ws.State == WebSocketState.Open && !cts.IsCancellationRequested)
        {
            try
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
                if (result.MessageType == WebSocketMessageType.Close) break;

                string jsonString = Encoding.UTF8.GetString(buffer, 0, result.Count);
                messageQueue.Enqueue(jsonString);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SDH] 수신 오류: {e.Message}");
                break;
            }
        }
    }

    // --- 3. 메인 스레드 처리 (Update) ---
    private void Update()
    {
        while (messageQueue.TryDequeue(out string json))
        {
            ProcessMessage(json);
        }
    }

    private void ProcessMessage(string json)
    {
        SocketMessage msg = JsonUtility.FromJson<SocketMessage>(json);

        switch (msg.type)
        {
            case "ROLE_ASSIGN":
                GameManagerUX.Instance.isHost = (msg.role == "HOST");
                Debug.Log($"[SDH] 내 역할 배정됨: {msg.role}");
                break;

            case "GAME_START":
                Debug.Log("[SDH] 게임이 시작되었습니다!");
                // TODO: 게임 시작 이벤트 발생
                break;

            case "UPDATE_CARD":
                // 상대방이 움직인 좌표 반영
                UpdateCardPosition(int.Parse(msg.cardId), msg.x, msg.y);
                break;

            // [추가됨] 상대방이 카드를 잡음 -> 나는 못 만지게 잠금
            case "GRAB_CARD":
                SetCardLockState(int.Parse(msg.cardId), true); 
                break;

            // [추가됨] 상대방이 카드를 놓음 -> 다시 만질 수 있게 해제
            case "RELEASE_CARD":
                SetCardLockState(int.Parse(msg.cardId), false);
                break;
        }
    }

    // --- 4. 송신 메서드들 ---

    public void SendCanvasSize()
    {
        // 사용자가 수정한 RatioAlignedCanvas 참조 유지
        if (RatioAlignedCanvas.InstanceWithoutCreate == null) return;
        
        SocketMessage msg = new SocketMessage
        {
            type = "INIT_CANVAS",
            width = RatioAlignedCanvas.InstanceWithoutCreate.xLength,
            height = RatioAlignedCanvas.InstanceWithoutCreate.yLength
        };
        SendJson(msg);
    }

    public void RequestGameStart()
    {
        if (!IsHost)
        {
            Debug.LogWarning("Host만 게임을 시작할 수 있습니다.");
            return;
        }
        SendJson(new SocketMessage { type = "START_GAME" });
    }

    public void SendCardMove(int cardId, Vector2 position)
    {
        SocketMessage msg = new SocketMessage
        {
            type = "MOVE_CARD",
            cardId = cardId.ToString(),
            x = position.x,
            y = position.y
        };
        SendJson(msg);
    }

    // [추가됨] 카드 잡았을 때 호출 (Touch Start)
    public void SendCardGrab(int cardId)
    {
        SocketMessage msg = new SocketMessage
        {
            type = "GRAB_CARD",
            cardId = cardId.ToString(),
        };
        SendJson(msg);
    }

    // [추가됨] 카드 놓았을 때 호출 (Touch End)
    public void SendCardRelease(int cardId)
    {
        SocketMessage msg = new SocketMessage
        {
            type = "RELEASE_CARD",
            cardId = cardId.ToString()
        };
        SendJson(msg);
    }

    private async void SendJson(SocketMessage msg)
    {
        if (ws.State != WebSocketState.Open) return;

        string json = JsonUtility.ToJson(msg);
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        
        await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cts.Token);
    }

    // --- 로컬 로직 (위치 이동 및 잠금 처리) ---

    private void UpdateCardPosition(int cardId, float x, float y)
    {
        NetworkCard netCard = SpawnCardOnCanvas.Instance.FindCardByID(cardId);
        if (netCard != null)
        {
            RectTransform rect = netCard.GetComponent<RectTransform>();
            if(rect != null) rect.anchoredPosition = new Vector2(x, y); 
        }
    }

    // [추가됨] 카드의 상호작용 잠금/해제 처리
    private void SetCardLockState(int cardId, bool isLocked)
    {
        // NetworkCard 컴포넌트를 찾아서 함수 호출
        NetworkCard netCard = SpawnCardOnCanvas.Instance.FindCardByID(cardId);
        if (netCard != null)
        {
            netCard.SetRemoteLock(isLocked);
            Debug.Log($"[SDH] 카드({cardId}) 잠금 상태 변경: {isLocked}");
        }
    }
}