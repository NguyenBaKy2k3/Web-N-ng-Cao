using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Models
{
    [Table("Reports")]
    public class ReportsModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int report_id { get; set; }
        public int reporter_id { get; set; }
        public int reported_user_id { get; set; }
        [Required]
        public string reason { get; set; }
        public DateTime created_at { get; set; }
    }
}
