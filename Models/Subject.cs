namespace QuanLyHocSinhCap3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Subject")]
    public class Subject
    {
        public int SubjectID { get; set; }
        public string Name { get; set; }

        public virtual List<Result> Results { get; set; }
        public virtual List<TeachingAssignment> TeachingAssignments { get; set; }
    }
}
