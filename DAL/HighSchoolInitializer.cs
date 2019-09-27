using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using QuanLyHocSinhCap3.Models;

namespace QuanLyHocSinhCap3.DAL
{
    public class HighSchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<HighSchoolContext>
    {
        protected override void Seed(HighSchoolContext context)
        {
            var students = new List<Student>
            {
            new Student{FirstName="Carson",LastName="Alexander",Birthday=DateTime.Parse("2005-09-01"),Address="An Duong Vuong"}
            };

            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();

            var accounts = new List<Account>
            {
                new Account {UserName="admin",Password="AQAAAAEAACcQAAAAEIE10Cf1qZcLx0H6ylIcOCWolIr1VaCyMi/KNQh/PHZ7PmmNwniLCIdPIPCdymWu6w=="}
                // password hashed from 123456
            };

            accounts.ForEach(a => context.Accounts.Add(a));
            context.SaveChanges();
        }
    }
}