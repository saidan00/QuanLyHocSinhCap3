using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HighSchoolManagerAPI.Models
{
    public class Account : IdentityUser
    {
        // [Key]
        // public int UserID { get; set; }

        // [Required]
        // [DisplayName("User Name")]
        // public string UserName { get; set; }

        // [Required]
        // [DataType(DataType.Password)]
        // public string Password { get; set; }

        public int? AccountRoleID { get; set; }

        public virtual AccountRole AccountRole { get; set; }
    }
}