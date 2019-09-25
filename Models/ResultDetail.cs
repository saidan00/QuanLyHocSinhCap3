namespace QuanLyHocSinhCap3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ResultDetail")]
    public class ResultDetail
    {
        public int ResultDetailID { get; set; }

        public double Mark { get; set; }

        public int Month { get; set; }

        public int ResultTypeID { get; set; }

        public int ResultID { get; set; }

        public virtual Result Result { get; set; }

        public virtual ResultType ResultType { get; set; }
    }
}
