namespace QuanLyHocSinhCap3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeachingAssignment")]
    public class TeachingAssignment
    {
        public int TeachingAssignmentID { get; set; }

        public int TeacherID { get; set; }

        public int ClassID { get; set; }

        public int SubjectID { get; set; }

        public virtual Class Class { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}
