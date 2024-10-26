using Dating.Data;
using Dating.Models;
using Microsoft.AspNetCore.Mvc;

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
            // Lấy tất cả các báo cáo liên quan đến người bị báo cáo
            var reports = _dbContext.Reports
                                    .Where(r => r.reported_user_id == reportedUserId)
                                    .ToList();

            // Đếm số lượng báo cáo
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


    }
}
