using System.Collections.Generic;

namespace HighSchoolManagerAPI.Models
{
    public class Semester
    {
        public int SemesterID { get; set; }

        public string Label { get; set; }

        public int Year { get; set; }

        public virtual List<Conduct> Conducts { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}