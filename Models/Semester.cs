namespace QuanLyHocSinhCap3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Semester")]
    public class Semester
    {
        public int SemesterID { get; set; }

        public string Label { get; set; }

        public int Year { get; set; }

        public virtual List<Conduct> Conducts { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}
