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
        }
    }
}