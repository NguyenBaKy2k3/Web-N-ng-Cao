using System.ComponentModel.DataAnnotations;

namespace Dating.Models
{
    public class DangNhapViewModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Không được để trống")]
        public string? Password { get; set; }
    }
}
