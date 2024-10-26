using System;
using Dating.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Models
{
    [Table("User_Profile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int profile_id { get; set; }

        public int user_profile_id { get; set; }

        [MaxLength(100)]
        public string occupation { get; set; }

        [MaxLength(30)]
        [RegularExpression("Độc thân|Đang trong mối quan hệ|Đã kết hôn|Phức tạp", ErrorMessage = "Tình trạng mối quan hệ không hợp lệ.")]
        public string relationship_status { get; set; }
        [MaxLength(20)]
        [RegularExpression("Nam|Nữ|Khác", ErrorMessage = "Giới tính không hợp lệ.")]
        public string gender_looking { get; set; }

        [MaxLength(100)]
        public string looking_for { get; set; }

        [MaxLength(100)]
        public string hobbies { get; set; }

        public decimal height { get; set; }

        public decimal weight { get; set; }

        public bool isApproved { get; set; }
    }
}
