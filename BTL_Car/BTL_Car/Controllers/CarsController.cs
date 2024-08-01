using BTL_Car.Data;
using BTL_Car.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_Car.Controllers
{
    public class CarsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CarsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var cars = _dbContext.Cars.ToList();
            return View(cars);
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("car_id,car_make,car_model,year_production,color,price_per_day,rating,license_plate,seats,transmission,fuel_type,image_url")] Cars car)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(car);
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Car created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Error creating car.";
            return View(car);
        }

        
        public IActionResult Edit(int id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("car_id,car_make,car_model,year_production,color,price_per_day,rating,license_plate,seats,transmission,fuel_type,image_url")] Cars car)
        {
            if (id != car.car_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(car);
                    _dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Car updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.car_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Error updating car.";
            return View(car);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            _dbContext.Cars.Remove(car);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Car deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _dbContext.Cars.Any(e => e.car_id == id);
        }

    }
}
