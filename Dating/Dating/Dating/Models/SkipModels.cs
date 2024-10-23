using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dating.Models
{
    [Table("Skippe")]
    public class SkipModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int skippe_id { get; set; }
        public int user_skip_id { get; set; } // Người dùng bỏ qua
        public int skippe_user_id { get; set; } // Người dùng bị bỏ qua


    }
}
