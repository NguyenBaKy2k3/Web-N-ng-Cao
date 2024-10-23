using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Models
{
    [Table ("Matches")]
    public class MatchModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int match_id { get; set; }
        public int user1_id { get; set; } //Người thích lại
        public int user2_id { get; set; } //Người đã thích từ trước
        public DateTime match_date { get; set; } = DateTime.Now;
    }
}
