using System.Collections.Generic;

namespace HighSchoolManagerAPI.Models
{
    public class Subject
    {
        public int SubjectID { get; set; }
        public string Name { get; set; }

        public virtual List<Result> Results { get; set; }
        public virtual List<TeachingAssignment> TeachingAssignments { get; set; }
    }
}