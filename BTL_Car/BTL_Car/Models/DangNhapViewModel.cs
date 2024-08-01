using System.ComponentModel.DataAnnotations;

namespace BTL_Car.Models
{
    public class DangNhapViewModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        public string? Username { get; set; }


        [Required(ErrorMessage = "Không được để trống")]
        public string? Password { get; set; }
    }
}
