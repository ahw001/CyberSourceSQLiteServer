using CybsClass.Cybersource.Models.SemiIntegrated;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CybsClass.WebApi.Service.Services.WssPosProcessing
{
    public class WssSemiIntPosConnect
    {
        public static async Task CreatePosConnection()
        {
            using (var ws = new ClientWebSocket())
            {
                Console.WriteLine("Connecting to Server");
                await ws.ConnectAsync(new Uri("wss://localhost:7014/poshub"), CancellationToken.None);
                Console.WriteLine("Connected");

                PosSetup setup = new()
                {
                    PosId = "123",
                    SetupCode = "ABC"
                };

                string jsonString = JsonSerializer.Serialize(setup);
                await SendJsonOverWebSocket(jsonString, ws);

                await ReceiveMessages(ws);

                // Properly close the WebSocket
                if (ws.State == WebSocketState.Open)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done sending data", CancellationToken.None);
                    Console.WriteLine("WebSocket closed");
                }
            }
        }

        private static async Task SendJsonOverWebSocket(string json, WebSocket webSocket)
        {

            if (webSocket.State == WebSocketState.Open)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private static async Task ReceiveMessages(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)); // 30 seconds timeout

            try
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationTokenSource.Token);
                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Message received from server: {receivedMessage}");

                    // Properly close the WebSocket
                    if (webSocket.State == WebSocketState.Open)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done sending data", CancellationToken.None);
                        Console.WriteLine("WebSocket closed");
                    }

                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Receive operation timed out.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
