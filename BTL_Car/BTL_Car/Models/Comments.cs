using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_Car.Models
{
    [Table ("tblComments")]
    public class Comments
    {
        [Key]
        public int comment_id { get; set; }

        [Required]
        public int user_comment_id { get; set; }

        [Required]
        public int car_id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string content { get; set; }

        [Required]
        public DateTime comment_date { get; set; }
    }
}
