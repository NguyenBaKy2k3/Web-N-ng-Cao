using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Models
{
    [Table("Ad_Min")]
    public class AdminModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int iAdmin { get; set; }
        public string sAdminName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int iRoleID { get; set; }
    }
}
