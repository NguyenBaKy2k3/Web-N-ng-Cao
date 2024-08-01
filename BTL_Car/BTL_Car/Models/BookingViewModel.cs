using System.ComponentModel.DataAnnotations;

namespace BTL_Car.Models
{
    public class BookingViewModel
    {
        public int CarId { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public DateTime BookingDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime RentalStartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime RentalEndDate { get; set; }
        public decimal TotalCost { get; set; } 
        [CompareDates(ErrorMessage = "Rental Start Date must be before Rental End Date.")]
        public bool CompareDates => RentalStartDate < RentalEndDate;
    }
    public class CompareDatesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (BookingViewModel)validationContext.ObjectInstance;
            if (model.RentalStartDate < model.RentalEndDate)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage ?? "Rental Start Date must be before Rental End Date.");
        }
    }
}
