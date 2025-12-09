using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkManagerHMD : SingletonObject<NetworkManagerHMD>
{

    // 상태 변수
    private ClientWebSocket ws = new ClientWebSocket();
    private CancellationTokenSource cts = new CancellationTokenSource();
    private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();

    public bool IsHost = false;
    public bool IsConnected => ws.State == WebSocketState.Open;

    private void OnDestroy()
    {
        cts.Cancel();
        if (ws != null && ws.State == WebSocketState.Open)
            ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
    }

    private async void Start()
    {
        await NetworkFunctionsProject.ConnectToServer(messageQueue, ws, cts);
    }

    // --- 3. 메인 스레드 처리 (Update) ---
    private void Update()
    {
        while (messageQueue.TryDequeue(out string json))
        {
            NetworkFunctionsProject.ProcessMessage(json);
        }
    }
}