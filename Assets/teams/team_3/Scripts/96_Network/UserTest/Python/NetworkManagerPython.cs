using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkManagerPython : MonoBehaviour
{
    // [설정] 서버 주소 (로컬 테스트 시 127.0.0.1, 실제 배포 시 IP 입력)
    [SerializeField] private string serverUrl = "ws://127.0.0.1:8000/ws";
    
    // [참조] 내 캔버스 (사이즈 측정용)
    public RectTransform myCanvasArea;

    // 상태 변수
    private ClientWebSocket ws = new ClientWebSocket();
    private CancellationTokenSource cts = new CancellationTokenSource();
    private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>(); // 메인 스레드 전달용 큐

    public bool IsHost = false;
    public bool IsConnected => ws.State == WebSocketState.Open;

    // 싱글톤 (어디서든 접근 가능하게)
    public static NetworkManagerPython Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

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
            Debug.Log("서버에 연결되었습니다.");

            // 연결 직후 수신 루프 시작
            _ = ReceiveLoop();

            // [요구사항 5] 내 캔버스 크기 전송
            SendCanvasSize();
        }
        catch (Exception e)
        {
            Debug.LogError($"연결 실패: {e.Message}");
        }
    }

    // --- 2. 메시지 수신 (백그라운드 스레드) ---
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
                
                // 받은 메시지를 큐에 넣음 (Update에서 처리하기 위해)
                messageQueue.Enqueue(jsonString);
            }
            catch (Exception e)
            {
                Debug.LogError($"수신 오류: {e.Message}");
                break;
            }
        }
    }

    // --- 3. 메인 스레드 처리 (Update) ---
    private void Update()
    {
        // 큐에 쌓인 메시지가 있으면 꺼내서 처리
        while (messageQueue.TryDequeue(out string json))
        {
            ProcessMessage(json);
        }
    }

    private void ProcessMessage(string json)
    {
        // JSON 파싱
        SocketMessage msg = JsonUtility.FromJson<SocketMessage>(json);

        switch (msg.type)
        {
            case "ROLE_ASSIGN":
                IsHost = (msg.role == "HOST");
                Debug.Log($"내 역할 배정됨: {msg.role}");
                break;

            case "GAME_START":
                Debug.Log("게임이 시작되었습니다!");
                // TODO: 게임 시작 UI 처리나 로직 호출
                break;

            case "UPDATE_CARD":
                // [요구사항 6] 상대방이 움직인 좌표 반영
                // msg.x, msg.y는 이미 서버에서 내 캔버스 비율에 맞게 변환된 값임
                UpdateCardPosition(msg.cardId, msg.x, msg.y);
                break;
        }
    }

    // --- 4. 송신 메서드들 ---

    // [요구사항 5] 캔버스 크기 전송
    public void SendCanvasSize()
    {
        if (myCanvasArea == null) return;
        
        SocketMessage msg = new SocketMessage
        {
            type = "INIT_CANVAS",
            width = myCanvasArea.rect.width,
            height = myCanvasArea.rect.height
        };
        SendJson(msg);
    }

    // [요구사항 4] 게임 시작 요청 (Host만 가능)
    public void RequestGameStart()
    {
        if (!IsHost)
        {
            Debug.LogWarning("Host만 게임을 시작할 수 있습니다.");
            return;
        }
        SendJson(new SocketMessage { type = "START_GAME" });
    }

    // [요구사항 6] 카드 이동 전송
    public void SendCardMove(string cardId, Vector2 position)
    {
        SocketMessage msg = new SocketMessage
        {
            type = "MOVE_CARD",
            cardId = cardId,
            x = position.x,
            y = position.y
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

    // --- 카드 업데이트 로직 (예시) ---
    private void UpdateCardPosition(string cardId, float x, float y)
    {
        // 씬에 있는 카드 객체를 찾아서 이동시킴
        // 실제 구현 시에는 Dictionary<string, GameObject>로 카드를 관리하는 게 좋음
        GameObject card = GameObject.Find(cardId);
        if (card != null)
        {
            // Canvas가 Screen Space라면 rectTransform.anchoredPosition 사용 추천
            // 여기서는 World 좌표 예시로 transform.position을 쓰거나 로직에 맞게 수정
            // 예: UI Canvas 상의 좌표라면 아래와 같이
             RectTransform rect = card.GetComponent<RectTransform>();
             if(rect != null) rect.anchoredPosition = new Vector2(x, y); 
        }
    }
}
