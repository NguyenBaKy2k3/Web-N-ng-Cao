using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_Car.Models
{
    [Table("tblCars")]
    public class Cars
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int car_id { get; set; }

        [Required(ErrorMessage = "Car make is required")]
        [StringLength(50, ErrorMessage = "Car make cannot be longer than 50 characters")]
        public string car_make { get; set; }

        [Required(ErrorMessage = "Car model is required")]
        [StringLength(100, ErrorMessage = "Car model cannot be longer than 100 characters")]
        public string car_model { get; set; }

        [Required(ErrorMessage = "Year of production is required")]
        [Range(1886, 2100, ErrorMessage = "Year of production must be between 1886 and 2100")]
        public int year_production { get; set; }

        [Required(ErrorMessage = "Color is required")]
        [StringLength(100, ErrorMessage = "Color cannot be longer than 100 characters")]
        public string color { get; set; }

        [Required(ErrorMessage = "Price per day is required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid price format")]
        public decimal price_per_day { get; set; } // Changed from float to double

        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string image_url { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public decimal rating { get; set; } // Changed from float to double

        [Required(ErrorMessage = "License plate is required")]
        [StringLength(10, ErrorMessage = "License plate cannot be longer than 10 characters")]
        public string license_plate { get; set; }

        [Required(ErrorMessage = "Seats are required")]
        [Range(1, 20, ErrorMessage = "Seats must be between 1 and 20")]
        public int seats { get; set; }

        [Required(ErrorMessage = "Transmission type is required")]
        [StringLength(10, ErrorMessage = "Transmission type cannot be longer than 10 characters")]
        public string transmission { get; set; }

        [Required(ErrorMessage = "Fuel type is required")]
        [StringLength(20, ErrorMessage = "Fuel type cannot be longer than 20 characters")]
        public string fuel_type { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^[0-9].{9}$",ErrorMessage = "Verifykey phải bắt đầu bằng 1 số và có 10 ký tự.")]
        public string verifyKey { get; set; }

    }
}
