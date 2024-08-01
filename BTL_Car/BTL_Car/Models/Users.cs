using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_Car.Models
{
    [Table("tblUsers")]
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; } // ID người dùng, tự tăng

        [Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Tên người dùng không được vượt quá 50 ký tự.")]
        public string username { get; set; }
        [Required(ErrorMessage = "Địa chỉ email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [StringLength(100, ErrorMessage = "Địa chỉ email không được vượt quá 100 ký tự.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        public string full_name { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
        public string phone_number { get; set; }

        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự.")]
        public string address { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime date_of_birth { get; set; }

        public int iUserRoleID { get; set; }

    }
}
