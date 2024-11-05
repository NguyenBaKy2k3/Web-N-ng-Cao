using Dating.Data;
using Dating.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Security.Claims;

namespace Dating.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AdminController(AppDbContext dbContext, ILogger<AdminController> logger)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult ProcessReports(int reportedUserId)
        {
            var reports = _dbContext.Reports
                                    .Where(r => r.reported_user_id == reportedUserId)
                                    .ToList();

            int reportCount = reports.Count;

            if (reportCount == 2)
            {
                var notification = new NotificationModels
                {
                    notification_receiver_id = reportedUserId,
                    admin_id = 1, 
                    notification_content = $"Bạn đã bị báo cáo 2 lần. Vui lòng kiểm tra cách hoạt động của mình.",
                    created_at = DateTime.Now
                };

                _dbContext.Notification.Add(notification);
            }
            else if (reportCount >= 3)
            {
                var userToDisable = _dbContext.Users.Find(reportedUserId);
                if (userToDisable != null)
                {
                    userToDisable.IsActive = false;
                }
            }

            _dbContext.SaveChanges();

            return RedirectToAction("Report", "Users"); 
        }


        public async Task<IActionResult> Index()
        {
            var feedbacks = await (from f in _dbContext.Feedback
                                   join u in _dbContext.Users on f.user_feeback_id equals u.user_id
                                   select new FeedbackViewModel
                                   {
                                       UserFeedbackId = f.user_feeback_id,
                                       Username = u.username,
                                       FeedbackContent = f.feedback_content
                                   }).ToListAsync();

            return View(feedbacks);
        }



        public IActionResult CreateNotification()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateNotification(NotificationModels notification, string notification_receiver_ids, bool SendToAll)
        {
            if (ModelState.IsValid && (SendToAll || !string.IsNullOrWhiteSpace(notification_receiver_ids)))
            {
                var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(adminIdClaim, out int adminId))
                {
                    notification.admin_id = adminId;
                    notification.created_at = DateTime.Now;

                    if (SendToAll)
                    {
                        // Gửi cho tất cả người dùng
                        var allUserIds = _dbContext.Users.Select(u => u.user_id).ToList();
                        foreach (var userId in allUserIds)
                        {
                            var newNotification = new NotificationModels
                            {
                                admin_id = adminId,
                                notification_receiver_id = userId,
                                notification_content = notification.notification_content,
                                created_at = notification.created_at,
                                is_read = false,
                            };
                            _dbContext.Notification.Add(newNotification);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(notification_receiver_ids))
                    {
                        // Xử lý chuỗi ID người nhận
                        var userIds = notification_receiver_ids
                            .Split(',')
                            .Select(id => id.Trim())
                            .Where(id => int.TryParse(id, out _))
                            .Select(int.Parse)
                            .ToList();

                        // Gửi cho các ID người nhận cụ thể
                        foreach (var userId in userIds)
                        {
                            var newNotification = new NotificationModels
                            {
                                admin_id = adminId,
                                notification_receiver_id = userId,
                                notification_content = notification.notification_content,
                                created_at = notification.created_at
                            };
                            _dbContext.Notification.Add(newNotification);
                        }
                    }

                    _dbContext.SaveChanges();
                    TempData["Message"] = "Đã gửi thông báo đến người dùng thành công.";
                    return RedirectToAction("CreateNotification");
                }
                ModelState.AddModelError("", "Không thể xác định ID Admin.");
            }

            return View(notification);
        }

    }
}
