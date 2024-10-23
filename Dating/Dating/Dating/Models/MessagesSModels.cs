using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Models
{
    [Table("MessagesS")]
    public class MessagesSModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int message_id { get; set; }
        public int sender_id { get; set; }
        public int receiver_id { get; set; }
        public string content { get; set; }
        public DateTime sent_at { get; set; }
    }
}
