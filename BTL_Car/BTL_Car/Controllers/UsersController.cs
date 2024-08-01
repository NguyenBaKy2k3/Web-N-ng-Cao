using BTL_Car.Data;
using BTL_Car.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BTL_Car.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _dbContext;

        public UsersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult TestDatabaseConnection()
        {
            var canConnect = _dbContext.Database.CanConnect();
            if (canConnect)
            {
                return Content("Connection successful!");
            }
            else
            {
                return Content("Connection failed!");
            }
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();

        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Users userViewModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên người dùng đã tồn tại chưa
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.username == userViewModel.username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên người dùng đã tồn tại, vui lòng chọn tên khác.");
                    return View(userViewModel);
                }

                var user = new Users
                {
                    username = userViewModel.username,
                    email = userViewModel.email,
                    password = userViewModel.password,
                    full_name = userViewModel.full_name,
                    phone_number = userViewModel.phone_number,
                    address = userViewModel.address,
                    date_of_birth = userViewModel.date_of_birth,
                    iUserRoleID = 2
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return RedirectToAction("Login"); // Redirect to home page or another page after successful registration
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
            var user = _dbContext.Users.Where(x => x.username == model.Username && x.password == model.Password).SingleOrDefault();

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.username.ToString());
                HttpContext.Session.SetInt32("Role", user.iUserRoleID);

                TempData["Role"] = user.iUserRoleID;

                if (user.iUserRoleID == 1)
                {
                    return RedirectToAction("Index", "Cars");
                }
                else
                {
                    return RedirectToAction("Index", "Booking");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu sai";
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
    }
}


/*using BTL_Car.Data;
using BTL_Car.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BTL_Car.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _dbContext;

        public UsersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Kiểm tra kết nối cơ sở dữ liệu
        public IActionResult TestDatabaseConnection()
        {
            var canConnect = _dbContext.Database.CanConnect();
            return Content(canConnect ? "Connection successful!" : "Connection failed!");
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Users userViewModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên người dùng đã tồn tại chưa
                var existingUser = _dbContext.Users
                    .FirstOrDefault(u => u.username == userViewModel.username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên người dùng đã tồn tại, vui lòng chọn tên khác.");
                    return View(userViewModel);
                }

                // Hash mật khẩu trước khi lưu vào cơ sở dữ liệu
                string hashedPassword = HashPassword(userViewModel.password);

                var user = new Users
                {
                    username = userViewModel.username,
                    email = userViewModel.email,
                    password = hashedPassword,
                    full_name = userViewModel.full_name,
                    phone_number = userViewModel.phone_number,
                    address = userViewModel.address,
                    date_of_birth = userViewModel.date_of_birth,
                    iUserRoleID = 2
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return RedirectToAction("Login");
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
            var user = _dbContext.Users
                .Where(x => x.username == model.Username)
                .SingleOrDefault();

            if (user != null && VerifyPassword(model.Password, user.password))
            {
                HttpContext.Session.SetString("UserName", user.username);
                HttpContext.Session.SetInt32("Role", user.iUserRoleID);

                TempData["Role"] = user.iUserRoleID;
                TempData["UserName"] = user.username;

                if (user.iUserRoleID == 1)
                {
                    return RedirectToAction("Index", "Cars");
                }
                else
                {
                    return RedirectToAction("Index", "Booking");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu sai";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("logout")]
        public IActionResult Logout()
        {
            TempData.Remove("UserName");
            TempData.Remove("Role");
            return RedirectToAction("Login"); // Hoặc chuyển hướng đến trang đăng nhập
        }

        // Hàm mã hóa mật khẩu
        private string HashPassword(string password)
        {
            // Generate a salt
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with the salt
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Return the salt and hashed password combined
            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        // Hàm kiểm tra mật khẩu
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            var parts = storedPassword.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[0]);
            var hashedPassword = parts[1];

            var hashedEnteredPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashedEnteredPassword == hashedPassword;
        }
    }
}
*/