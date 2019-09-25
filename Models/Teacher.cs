namespace QuanLyHocSinhCap3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Teacher")]
    public class Teacher
    {
        public int TeacherID { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public virtual List<Class> Classes { get; set; }
        public virtual List<TeachingAssignment> TeachingAssignments { get; set; }
    }
}
