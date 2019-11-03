using System;
using System.Linq;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence
{
    public class DataSeed
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
                },
                new Teacher
                {
                    Name = "Manager",
                    Birthday = DateTime.Parse("1998-11-30")
                },
                new Teacher
                {
                    Name = "Teacher",
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

            // seed admmin
            var teacher = context.Teachers.Where(t => t.Name.Equals("Admin")).FirstOrDefault();

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

            // Seed manager
            var teacher2 = context.Teachers.Where(t => t.Name.Equals("Manager")).FirstOrDefault();

            var user2 = new ApplicationUser
            {
                UserName = "manager",
                NormalizedUserName = "MANAGER",
                TeacherID = teacher.TeacherID
            };

            if (!context.Users.Any(u => u.UserName == user2.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user2, "123456");
                user2.PasswordHash = hashed;

                context.Users.Add(user2);
            }
            context.SaveChanges();

            var teacher3 = context.Teachers.Where(t => t.Name.Equals("Teacher")).FirstOrDefault();

            var user3 = new ApplicationUser
            {
                UserName = "teacher",
                NormalizedUserName = "TEACHER",
                TeacherID = teacher.TeacherID
            };

            if (!context.Users.Any(u => u.UserName == user3.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user3, "123456");
                user3.PasswordHash = hashed;

                context.Users.Add(user3);
            }
            context.SaveChanges();

            // set role for user
            var aUser = context.Users.Where(u => u.UserName.Equals("Admin")).FirstOrDefault();
            var aRole = context.Roles.Where(r => r.Name.Equals("Admin")).FirstOrDefault();
            var userRole = new IdentityUserRole<string>
            {
                UserId = aUser.Id,
                RoleId = aRole.Id
            };
            context.UserRoles.Add(userRole);
            context.SaveChanges();

            var aUser2 = context.Users.Where(u => u.UserName.Equals("Manager")).FirstOrDefault();
            var aRole2 = context.Roles.Where(r => r.Name.Equals("Manager")).FirstOrDefault();
            var userRole2 = new IdentityUserRole<string>
            {
                UserId = aUser2.Id,
                RoleId = aRole2.Id
            };
            context.UserRoles.Add(userRole2);
            context.SaveChanges();

            var aUser3 = context.Users.Where(u => u.UserName.Equals("Teacher")).FirstOrDefault();
            var aRole3 = context.Roles.Where(r => r.Name.Equals("Teacher")).FirstOrDefault();
            var userRole3 = new IdentityUserRole<string>
            {
                UserId = aUser3.Id,
                RoleId = aRole3.Id
            };
            context.UserRoles.Add(userRole3);
            context.SaveChanges();

            // seed grades
            var grades = new Grade[]
            {
                new Grade
                {
                    Name = "12"
                },
                new Grade
                {
                    Name = "11"
                },
                new Grade
                {
                    Name = "10"
                }
            };

            foreach (Grade g in grades)
            {
                context.Grades.Add(g);
            }
            context.SaveChanges();
        }
    }
}