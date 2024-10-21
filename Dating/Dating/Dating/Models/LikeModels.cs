using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Models
{
    [Table("Likes")]
    public class LikeModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int like_id { get; set; }
        public int userlike_id { get; set; } // Người dùng thực hiện lượt thích
        public int liked_user_id { get; set; } // Người dùng được thích

        public DateTime created_at { get; set; } // Thời gian thích

    }
}
