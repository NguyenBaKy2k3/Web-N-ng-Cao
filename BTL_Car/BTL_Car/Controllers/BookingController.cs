using Microsoft.AspNetCore.Mvc;
using BTL_Car.Models;
using BTL_Car.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BTL_Car.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _dbContext;
        public BookingController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        const string LISTBOOKING = "MY_LIST";
        [HttpGet]
        public IActionResult Book(int id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            var viewModel = new BookingViewModel
            {
                CarId = car.car_id,
                CarMake = car.car_make,
                CarModel = car.car_model,
                BookingDate = DateTime.Now
            };

            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(BookingViewModel booking)
        {
            if (booking == null)
            {
                ModelState.AddModelError("", "Dữ liệu đặt xe không hợp lệ.");
                return View(booking);
            }

            if (booking.CarId <= 0)
            {
                ModelState.AddModelError("", "ID của xe không hợp lệ.");
                return View(booking);
            }

            var car = _dbContext.Cars.Find(booking.CarId);
            if (car == null)
            {
                ModelState.AddModelError("", "Xe không tồn tại.");
                return View(booking);
            }

            if (ModelState.IsValid)
            {
                var existingBooking = _dbContext.Bookings
                    .Any(b => b.car_booking_id == booking.CarId &&
                              ((b.rental_start_date <= booking.RentalEndDate) && (b.rental_end_date >= booking.RentalStartDate)));

                if (existingBooking)
                {
                    ModelState.AddModelError("", "Xe đã được đặt trong khoảng thời gian này.");
                    return View(booking);
                }

                var rentalDays = (booking.RentalEndDate - booking.RentalStartDate).Days;
                var totalCost = rentalDays * car.price_per_day;
                var userName = HttpContext.Session.GetString("UserName"); 
                var user = _dbContext.Users.FirstOrDefault(u => u.username == userName);

                if (user == null)
                {
                    ModelState.AddModelError("", "Không thể xác định người dùng.");
                    return View(booking);
                }

                int userId = user.user_id;

                var item = new Booking
                {

                    user_booking_id = userId,
                    car_booking_id = booking.CarId,
                    booking_date = DateTime.Now,
                    rental_start_date = booking.RentalStartDate,
                    rental_end_date = booking.RentalStartDate,
                    total_cost = (decimal)totalCost,
                    status_car = "Đã đặt"
                };
                try
                {
                    _dbContext.Bookings.Add(item);
                    _dbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Đặt xe thành công!";
                    return RedirectToAction("hiendsBooking");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                    return View(booking);
                }
            }
            return View(booking); ;
        }
        public IActionResult hiendsBooking()
        {
            var userName = HttpContext.Session.GetString("UserName"); 
            var user = _dbContext.Users.FirstOrDefault(u => u.username == userName);

            if (user == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var list = _dbContext.Bookings.Where(x => x.user_booking_id == user.user_id).ToList();
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(list);
        }

        private string GetCurrentUserName()
        {
            return HttpContext.Session.GetString("UserName");
        }

        
        public IActionResult Cancel(int id)
        {
            var booking = _dbContext.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            _dbContext.Bookings.Remove(booking);
            _dbContext.SaveChanges();
            //TempData["Cancel"] = "Hủy thành công!";
            return RedirectToAction("hiendsBooking");
        }

        /*public IActionResult Index()
        {
            var cars = _dbContext.Cars.ToList(); // Lấy danh sách xe từ cơ sở dữ liệu
                                                 //var cars =_dbContext.Bookings.ToList();
            return View(cars);
        }*/


        public IActionResult Index(string sortOrder, decimal? minPrice, decimal? maxPrice, int? minSeats, int? maxSeats)
        {
            ViewBag.PriceSort = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.SeatSort = sortOrder == "seats" ? "seats_desc" : "seats";

            var cars = from c in _dbContext.Cars
                       select c;

            if (minPrice.HasValue)
            {
                cars = cars.Where(c => c.price_per_day >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                cars = cars.Where(c => c.price_per_day <= maxPrice.Value);
            }

            if (minSeats.HasValue)
            {
                cars = cars.Where(c => c.seats >= minSeats.Value);
            }

            if (maxSeats.HasValue)
            {
                cars = cars.Where(c => c.seats <= maxSeats.Value);
            }

            switch (sortOrder)
            {
                case "price":
                    cars = cars.OrderBy(c => c.price_per_day);
                    break;
                case "price_desc":
                    cars = cars.OrderByDescending(c => c.price_per_day);
                    break;
                case "seats":
                    cars = cars.OrderBy(c => c.seats);
                    break;
                case "seats_desc":
                    cars = cars.OrderByDescending(c => c.seats);
                    break;
                default:
                    cars = cars.OrderBy(c => c.car_make);
                    break;
            }

            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;
            ViewData["MinSeats"] = minSeats;
            ViewData["MaxSeats"] = maxSeats;

            return View(cars.ToList());
        }



        public int GetNewBookingsCount()
        {
            var userId = User.Identity.Name;
            int userIdInt;
            if (int.TryParse(userId, out userIdInt))
            {
                return _dbContext.Bookings.Count(b => b.user_booking_id == userIdInt && b.status_car == "Đã đặt");
            }
            return 0;
        }


        public IActionResult GetNewBookingsCountJson()
        {
            var count = GetNewBookingsCount();
            return Json(new { count });
        }



        public IActionResult Details(int id)
        {
            var car = _dbContext.Cars
                .FirstOrDefault(m => m.car_id == id);

            if (car == null)
            {
                return NotFound();
            }

            var comments = _dbContext.Comments
                .Where(c => c.car_id == id)
                .ToList();

            var userNames = _dbContext.Users
                .ToDictionary(u => u.user_id, u => u.username);

            var viewModel = new CarDetailsViewModel
            {
                Car = car,
                Comments = comments
            };

            ViewBag.UserNames = userNames;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddComment(int carId, string content)
        {
            
            var userName = HttpContext.Session.GetString("UserName");

            
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Users");
            }

            
            var user = _dbContext.Users.FirstOrDefault(u => u.username == userName);

            
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var userId = user.user_id;

            var comment = new Comments
            {
                car_id = carId,
                content = content,
                comment_date = DateTime.Now,
                user_comment_id = userId 
            };

            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();

            return RedirectToAction("Details", new { id = carId });
        }



    }
}
