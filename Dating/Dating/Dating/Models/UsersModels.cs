using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dating.Models
{
    [Table("Users")]
    public class UsersModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; } // ID người dùng, tự tăng

        [Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Tên người dùng không quá 50 ký tự.")]
        public string username { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(12, ErrorMessage = "Số điện thoại không quá 12 ký tự.")]
        public string sdt { get; set; } // Số điện thoại

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(100, ErrorMessage = "Email không quá 100 ký tự.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string password { get; set; }

        public int iUsersRoleID { get; set; }

        // Các trường bổ sung
        [Required(ErrorMessage = "Giới tính là bắt buộc.")]
        [StringLength(10, ErrorMessage = "Giới tính không hợp lệ.")]
        public string gender { get; set; } // Giới tính: male, female, other

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        [DataType(DataType.Date)]
        public DateTime date_of_birth { get; set; } // Ngày sinh

        [StringLength(500, ErrorMessage = "Tiểu sử không được quá 500 ký tự.")]
        public string bio { get; set; } // Thông tin tiểu sử

        [StringLength(255)]
        [ValidateNever]
        public string profile_picture { get; set; } // Đường dẫn ảnh đại diện

        [StringLength(100)]
        public string location { get; set; } // Địa điểm

        public double latitude { get; set; }  // Vĩ độ

        public double longitude { get; set; }  // Kinh độ
        public bool IsActive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; } = DateTime.Now; // Thời gian tạo tài khoản

        [NotMapped]
        [ValidateNever]
        public IFormFile ProfileImage { get; set; } // Tệp ảnh đại diện, không lưu trong cơ sở dữ liệu

        // Tính tuổi nhưng không lưu vào cơ sở dữ liệu
        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - date_of_birth.Year;
                if (date_of_birth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
