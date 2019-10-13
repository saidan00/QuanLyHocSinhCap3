using Microsoft.AspNetCore.Identity;

namespace HighSchoolManagerAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int TeacherID { get; set; }

        public Teacher Teacher { get; set; }
    }
}