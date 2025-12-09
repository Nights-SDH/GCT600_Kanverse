using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class NetworkFunctionsProject
{
    public const string serverUrl = "wss://arpserver-production.up.railway.app/ws";
    public static void ProcessMessage(string json)
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
        }
    }

    public static async Task ConnectToServer(ConcurrentQueue<string> messageQueue, ClientWebSocket ws, CancellationTokenSource cts)
    {
        try
        {
            ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri(NetworkFunctionsProject.serverUrl), cts.Token);
            Debug.Log("[SDH] 서버에 연결되었습니다.");

            _ = NetworkFunctionsProject.ReceiveLoop(messageQueue, ws, cts);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SDH] 연결 실패: {e.Message}");
        }
    }

    // --- 4. 송신 메서드들 ---
    public static void RequestGameStart(bool IsHost, ClientWebSocket ws, CancellationTokenSource cts)
    {
        if (!IsHost)
        {
            Debug.LogWarning("Host만 게임을 시작할 수 있습니다.");
            return;
        }
        SendJson(new SocketMessage { type = "START_GAME" }, ws, cts);
    }

    private static async void SendJson(SocketMessage msg, ClientWebSocket ws, CancellationTokenSource cts)
    {
        if (ws.State != WebSocketState.Open) return;

        string json = JsonUtility.ToJson(msg);
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        
        await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cts.Token);
    }

    public static async Task ReceiveLoop(ConcurrentQueue<string> messageQueue, ClientWebSocket ws, CancellationTokenSource cts)
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
}
