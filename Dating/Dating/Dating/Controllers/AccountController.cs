using Google;
using Dating.Data;
using Dating.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dating.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int? userIndex, string currentUserId)
        {
            if (string.IsNullOrEmpty(currentUserId))
            {
                // Xử lý lỗi, có thể chuyển hướng đến trang đăng nhập hoặc hiển thị thông báo lỗi
                return RedirectToAction("Login");
            }

            var users = _context.Users
                .Where(u => u.user_id.ToString() != currentUserId)
                .ToList();

            userIndex ??= 0;
            if (userIndex < 0 || userIndex >= users.Count)
            {
                userIndex = 0;
            }

            var currentUser = users[userIndex.Value];
            ViewBag.UserIndex = userIndex;
            ViewBag.UserAge = CalculateAge(currentUser.date_of_birth); // Tính tuổi và truyền qua ViewBag
            return View(currentUser);
        }

        private int CalculateAge(DateTime birthdate)
        {
            int age = DateTime.Now.Year - birthdate.Year;
            if (DateTime.Now.DayOfYear < birthdate.DayOfYear)
                age -= 1;

            return age;
        }

        [HttpGet]
        public IActionResult Swipe(bool like, int userIndex, string currentUserId)
        {
            if (string.IsNullOrEmpty(currentUserId))
            {
                // Xử lý lỗi
                return RedirectToAction("Login");
            }

            // Xử lý logic Thích hoặc Bỏ qua tại đây, ví dụ: lưu vào cơ sở dữ liệu
            // Chuyển sang người dùng tiếp theo
            return RedirectToAction("Index", new { userIndex = userIndex + 1, currentUserId = currentUserId });
        }
    }
}
