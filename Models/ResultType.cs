namespace QuanLyHocSinhCap3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ResultType")]
    public class ResultType
    {
        public int ResultTypeID { get; set; }

        public string Name { get; set; }

        public int Coefficient { get; set; }

        public virtual List<ResultDetail> ResultDetails { get; set; }
    }
}
