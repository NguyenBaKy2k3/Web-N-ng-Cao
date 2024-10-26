using Dating.Data;
using Microsoft.AspNetCore.SignalR;

namespace Dating.Views.signalR.hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string senderId, string receiverId, string messageContent)
        {
            // Lưu tin nhắn vào cơ sở dữ liệu
            var message = new Models.MessagesSModels
            {
                sender_id = int.Parse(senderId),
                receiver_id = int.Parse(receiverId),
                content = messageContent,
                sent_at = DateTime.UtcNow
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Gửi tin nhắn đến người nhận
            await Clients.User(receiverId).SendAsync("ReceiveMessage", message);
        }
    }
}
