using System.ComponentModel.DataAnnotations;

namespace HighSchoolManagerAPI.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}