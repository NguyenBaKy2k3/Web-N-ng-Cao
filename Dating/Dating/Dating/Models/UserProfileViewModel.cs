using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Models
{
    public class UserProfileViewModel
    {
        public int ProfileId { get; set; } // ID hồ sơ
        public string Occupation { get; set; } // Nghề nghiệp
        public string RelationshipStatus { get; set; } // Tình trạng mối quan hệ
        public string LookingFor { get; set; } // Mục tiêu tìm kiếm
        public string Hobbies { get; set; } // Sở thích
        public decimal Height { get; set; } // Chiều cao
        public decimal Weight { get; set; } // Cân nặng
        public bool IsApproved { get; set; } // Trạng thái duyệt

        // Các thông tin từ UsersModels
        public string Username { get; set; } // Tên người dùng
        public string Gender { get; set; } // Giới tính
        public string Bio { get; set; } // Tiểu sử
        public string ProfilePicture { get; set; } // Ảnh đại diện
        public int Age { get; set; }
        public string Location  { get; set; }

        [NotMapped]
        [ValidateNever]
        public IFormFile Profileimage { get; set; }
    }
}
