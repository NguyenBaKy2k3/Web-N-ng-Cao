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
        public int ProfileId { get; set; }

        [MaxLength(100)]
        public string Occupation { get; set; }

        [MaxLength(20)]
        [RegularExpression("single|in a relationship|married|complicated", ErrorMessage = "Tình trạng mối quan hệ không hợp lệ.")]
        public string RelationshipStatus { get; set; }

        [MaxLength(100)]
        public string LookingFor { get; set; }

        [MaxLength(100)]
        public string Hobbies { get; set; }

        public decimal Height { get; set; }

        public decimal Weight { get; set; }
    }
}
