using Dating.Data;
using System.Net.Mail;
using Dating.Models;
using Dating.Email;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Dating.Controllers
{
    public class UsersController : Controller
    {

        private readonly AppDbContext _dbContext;
        private readonly ILogger<UsersController> _logger;

        public UsersController(AppDbContext dbContext, ILogger<UsersController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UsersModels userViewModel, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (ModelState.IsValid)
            {
                var existingEmail = _dbContext.Users.FirstOrDefault(u => u.email == userViewModel.email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("Email", "Email người dùng đã tồn tại, vui lòng chọn Email khác.");
                    return View(userViewModel);
                }

                var user = new UsersModels
                {
                    username = userViewModel.username,
                    sdt = userViewModel.sdt,
                    email = userViewModel.email,
                    password = userViewModel.password,
                    gender = userViewModel.gender,
                    date_of_birth = userViewModel.date_of_birth,
                    bio = userViewModel.bio,
                    location = userViewModel.location,
                    latitude = userViewModel.latitude,
                    longitude = userViewModel.longitude,
                    iUsersRoleID = 2,
                    created_at = DateTime.Now
                };

                if (userViewModel.ProfileImage != null && userViewModel.ProfileImage.Length > 0)
                {
                    string folderPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = Path.Combine(folderPath, Path.GetFileName(userViewModel.ProfileImage.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        userViewModel.ProfileImage.CopyTo(stream);
                    }
                    user.profile_picture = "/uploads/" + Path.GetFileName(userViewModel.ProfileImage.FileName);
                }

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(userViewModel);
        }




        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(DangNhapViewModel model)
        {
            try
            {
                var user = await _dbContext.Users
                    .SingleOrDefaultAsync(x => x.email == model.Email && x.password == model.Password);

                if (user != null)
                {
                    // Tạo mã xác nhận
                    var verificationCode = new Random().Next(100000, 999999).ToString();

                    // Lấy dịch vụ EmailService từ DI container
                    var emailService = HttpContext.RequestServices.GetService<EmailService>();
                    if (emailService != null)
                    {
                        // Gửi mã xác nhận qua email
                        await emailService.SendEmailAsync(user.email, "Mã xác nhận đăng nhập", $"Mã xác nhận của bạn là: {verificationCode}");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Không thể gửi email xác nhận. Vui lòng thử lại.";
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()), // Hoặc trường ID tương ứng
                        new Claim(ClaimTypes.Email, user.email) // Thêm email vào Claims
                        // Thêm các Claims khác nếu cần
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


                    // Lưu mã xác nhận vào session
                    HttpContext.Session.SetString("VerificationCode", verificationCode);

                    // Điều hướng người dùng đến trang xác nhận mã
                    return RedirectToAction("VerifyCode");
                }
                else
                {
                    // Tài khoản hoặc mật khẩu không đúng
                    ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu sai";
                }
            }
            catch (Exception ex)
            {
                // Log lỗi để dễ dàng debug
                Console.WriteLine($"Đã xảy ra lỗi trong quá trình đăng nhập: {ex.Message}");
                ViewBag.ErrorMessage = "Đã xảy ra lỗi, vui lòng thử lại.";
            }

            return View();
        }





        [HttpGet]
        [Route("verifycode")]
        public IActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        [Route("verifycode")]
        public IActionResult VerifyCode(string enteredCode)
        {
            var sessionCode = HttpContext.Session.GetString("VerificationCode");

            if (sessionCode == enteredCode)
            {
                ViewBag.SuccessMessage = "Xác nhận mã thành công! Bạn sẽ được chuyển hướng trong giây lát.";

                // Xóa mã xác nhận khỏi session
                HttpContext.Session.Remove("VerificationCode");
            }
            else
            {
                ViewBag.ErrorMessage = "Mã xác nhận không chính xác";
            }

            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Xóa dữ liệu phiên
            HttpContext.Session.Clear();

            // Xóa cookie xác thực
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).GetAwaiter().GetResult();

            // Xóa thông tin xác thực của người dùng
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Chuyển hướng về trang đăng nhập
            return RedirectToAction("Login");
        }



        [HttpGet]
        [Route("forgotpassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.email == email);

                if (user == null)
                {
                    ViewBag.ErrorMessage = "Email không tồn tại trong hệ thống.";
                    return View();
                }

                // Tạo mật khẩu mới
                var newPassword = GenerateRandomPassword();

                // Cập nhật mật khẩu mới trong cơ sở dữ liệu
                user.password = newPassword;
                await _dbContext.SaveChangesAsync();

                // Lấy dịch vụ EmailService từ DI container
                var emailService = HttpContext.RequestServices.GetService<EmailService>();
                if (emailService != null)
                {
                    // Gửi email với mật khẩu mới
                    await emailService.SendEmailAsync(user.email, "Cấp lại mật khẩu", $"Mật khẩu mới của bạn là: {newPassword}");
                    TempData["Message"] = "Mật khẩu mới đã được gửi đến email của bạn. Hãy kiểm tra email và đăng nhập lại.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể gửi email cấp lại mật khẩu. Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                // Log lỗi để dễ dàng debug
                Console.WriteLine($"Đã xảy ra lỗi trong quá trình cấp lại mật khẩu: {ex.Message}");
                ViewBag.ErrorMessage = "Đã xảy ra lỗi, vui lòng thử lại.";
            }

            return View();
        }


        private string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public async Task<IActionResult> UserList()
        {
            // Lấy email của người dùng hiện tại từ Claims
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);

            // Tìm người dùng hiện tại dựa trên email
            var currentUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.email == currentUserEmail);

            // Lấy ID người dùng hiện tại
            var currentUserId = currentUser?.user_id; // Trường hợp không tìm thấy người dùng, currentUserId sẽ là null

            // Lấy danh sách người dùng mà không bao gồm người dùng hiện tại
            var users = await _dbContext.Users
                .Where(u => u.user_id != currentUserId) // Lọc ra người dùng không phải là người đang đăng nhập
                .ToListAsync();

            return View(users);
        }



    }
}
