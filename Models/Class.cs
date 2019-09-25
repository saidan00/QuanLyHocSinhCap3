namespace QuanLyHocSinhCap3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Class")]
    public class Class
    {
        public int ClassID { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public int Size { get; set; }

        public int GradeID { get; set; }

        public int? HeadTeacherID { get; set; }


        public virtual Grade Grade { get; set; }
        public virtual Teacher HeadTeacher { get; set; }
        public virtual List<Student> Students { get; set; }
        public virtual List<TeachingAssignment> TeachingAssignments { get; set; }
    }
}
