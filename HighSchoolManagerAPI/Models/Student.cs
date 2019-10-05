using System;
using System.Collections.Generic;

namespace HighSchoolManagerAPI.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        public int? ClassID { get; set; }

        public virtual Class Class { get; set; }
        public virtual List<Conduct> Conducts { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}