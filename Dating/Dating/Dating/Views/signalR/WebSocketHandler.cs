using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dating.Models;
using Dating.Data;

public class WebSocketHandler
{
    private readonly WebSocket _webSocket;
    private readonly AppDbContext _dbContext;
    private readonly int _currentUserId;

    public WebSocketHandler(WebSocket webSocket, AppDbContext dbContext, int currentUserId)
    {
        _webSocket = webSocket;
        _dbContext = dbContext;
        _currentUserId = currentUserId;
    }

    public async Task Handle()
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            string messageContent = Encoding.UTF8.GetString(buffer, 0, result.Count);

            // Lưu tin nhắn vào cơ sở dữ liệu
            var message = new MessagesSModels
            {
                sender_id = _currentUserId,
                receiver_id = int.Parse(GetValueFromJson(messageContent, "receiver_id")),
                content = GetValueFromJson(messageContent, "content"),
                sent_at = DateTime.Now
            };
            _dbContext.Add(message);
            await _dbContext.SaveChangesAsync();

            // Gửi lại tin nhắn tới client
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

            result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await _webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }

    private string GetValueFromJson(string json, string key)
    {
        var keyValuePairs = json.Trim('{', '}').Split(',');
        foreach (var kvp in keyValuePairs)
        {
            var pair = kvp.Split(':');
            if (pair[0].Trim('"') == key)
            {
                return pair[1].Trim('"');
            }
        }
        return string.Empty;
    }
}
