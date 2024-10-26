using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Models
{
    [Table ("Feedback")]
    public class FeedbackModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int feedback_id { get; set; }
        public int user_feeback_id { get; set; }
        public string feedback_content { get; set; }
    }
}
