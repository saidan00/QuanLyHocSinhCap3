using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HighSchoolManagerAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HighSchoolManagerAPI.Data
{
    public class DbInitializer
    {
        public static void Initialize(HighSchoolContext context)
        {
            // Look for any teachers
            if (context.Teachers.Any())
            {
                return;   // DB has been seeded
            }

            // seed teachers
            var teachers = new Teacher[]
            {
                new Teacher
                {
                    Name = "Admin",
                    Birthday = DateTime.Parse("1998-11-30")
                }
            };

            foreach (Teacher t in teachers)
            {
                context.Teachers.Add(t);
            }
            context.SaveChanges();

            // seed roles
            var roles = new IdentityRole[]
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new IdentityRole
                {
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                }
            };

            foreach (IdentityRole role in roles)
            {
                context.Roles.Add(role);
            }
            context.SaveChanges();

            // seed users
            var teacher = context.Teachers.FirstOrDefault();

            var user = new ApplicationUser
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                TeacherID = teacher.TeacherID
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "123456");
                user.PasswordHash = hashed;

                context.Users.Add(user);
            }
            context.SaveChanges();

            // set role for user
            var aUser = context.Users.FirstOrDefault();
            var aRole = context.Roles.Where(r => r.Name.Equals("Admin")).FirstOrDefault();
            var userRole = new IdentityUserRole<string>
            {
                UserId = aUser.Id,
                RoleId = aRole.Id
            };
            context.UserRoles.Add(userRole);
            context.SaveChanges();
        }
    }
}