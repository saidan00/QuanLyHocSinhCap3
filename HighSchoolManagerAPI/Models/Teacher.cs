using System;
using System.Collections.Generic;

namespace HighSchoolManagerAPI.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public virtual List<Class> Classes { get; set; }
        public virtual List<TeachingAssignment> TeachingAssignments { get; set; }
    }
}