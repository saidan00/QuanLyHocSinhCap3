using System.ComponentModel.DataAnnotations;

namespace HighSchoolManagerAPI.Models
{
    public class CreateRoleModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}