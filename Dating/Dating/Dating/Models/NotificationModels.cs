using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Models
{
    [Table("tblNotification")]
    public class NotificationModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int notification_id { get; set; }
        public int notification_receiver_id { get; set; }
        public int admin_id { get; set; }
        public string notification_content { get; set; }
        public DateTime created_at { get; set; }
    }
}
