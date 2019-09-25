namespace QuanLyHocSinhCap3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Grade")]
    public class Grade
    {
        public int GradeID { get; set; }
        public string Name { get; set; }

        public virtual List<Class> Classes { get; set; }
    }
}
