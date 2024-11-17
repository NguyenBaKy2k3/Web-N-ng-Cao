using Dating.Controllers;
using Dating.Data;
using Dating.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Dating.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _dbContext;
        private static readonly ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();

        public ChatHub(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(int receiverId, string message)
        {
            try
            {
                // Lấy HttpContext từ Hub context
                var httpContext = Context.GetHttpContext();

                // Lấy UserId từ session
                var senderId = httpContext.Session.GetInt32("UserId");

                if (senderId == null)
                {
                    await Clients.Caller.SendAsync("ReceiveMessage", "Server", "User not found!");
                    return;
                }

                var receiver = await _dbContext.Users.FindAsync(receiverId);
                if (receiver != null)
                {
                    // Lưu tin nhắn vào database
                    var newMessage = new MessagesSModels
                    {
                        sender_id = senderId.Value,
                        receiver_id = receiverId,
                        content = message,
                        sent_at = DateTime.Now
                    };

                    _dbContext.Messages.Add(newMessage);
                    await _dbContext.SaveChangesAsync();

                    // Gửi tin nhắn đến người nhận qua SignalR
                    //Console.WriteLine($"Sending message to user {receiverId.ToString()} (UserIdentifier: {Context.UserIdentifier})");
                    //await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, message);
                    await Clients.All.SendAsync("ReceiveMessage", senderId, message);

                }
                else
                {
                    await Clients.Caller.SendAsync("ReceiveMessage", "Server", "Receiver not found!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                await Clients.Caller.SendAsync("ReceiveMessage", "Server", "An error occurred while sending the message.");
            }
        }

    }
}
