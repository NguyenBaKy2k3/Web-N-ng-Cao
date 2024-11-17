using Dating.Data;
using Dating.Hubs;
using Dating.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Dating.Controllers
{
    public class MessagesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(AppDbContext dbContext, IHubContext<ChatHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        // Action để hiển thị trang nhắn tin với ID người dùng
        [HttpGet]
        public IActionResult Chat(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.user_id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public async Task<IActionResult> SendMessage(int receiverId, string message)
        {
            var senderId = HttpContext.Session.GetInt32("UserId"); // Lấy thông tin người gửi từ session
            Console.WriteLine($"SenderId: {senderId}, ReceiverId: {receiverId}, Message: {message}");
            if (senderId == null)
            {
                return Unauthorized(); // Trả về lỗi nếu không tìm thấy người dùng trong session
            }

            var receiver = _dbContext.Users.FirstOrDefault(u => u.user_id == receiverId); // Lấy thông tin người nhận từ database
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

                _dbContext.Messages.Add(newMessage); // Thêm tin nhắn vào cơ sở dữ liệu
                await _dbContext.SaveChangesAsync(); // Lưu thay đổi

                // Gửi tin nhắn đến người nhận qua SignalR
                await _hubContext.Clients.User(receiver.user_id.ToString()).SendAsync("ReceiveMessage", senderId, message);
            }

            // Sau khi gửi tin nhắn, chuyển hướng người dùng đến trang chat
            return RedirectToAction("Chat", new { id = receiverId });
        }

        [HttpGet]
        public async Task<IActionResult> GetChatHistory(int receiverId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var messages = await _dbContext.Messages
                .Where(m => (m.sender_id == userId && m.receiver_id == receiverId) ||
                            (m.sender_id == receiverId && m.receiver_id == userId))
                .OrderBy(m => m.sent_at)
                .Select(m => new {
                    sender_id = m.sender_id,
                    message_text = m.content,
                    sent_at = m.sent_at.ToString("HH:mm:ss") // Định dạng thời gian
                })
                .ToListAsync();
            ViewBag.UserId = userId;
            return Json(messages);
        }
    }
}
