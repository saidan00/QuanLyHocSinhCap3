using System.Linq;
using HighSchoolManagerAPI.Data;

namespace HighSchoolManagerAPI.Helpers
{
    // CHECK IF ENTITY EXISTS
    public class ExistHelper
    {
        private readonly HighSchoolContext _context;

        public ExistHelper(HighSchoolContext context)
        {
            _context = context;
        }

        public bool StudentExists(int? id)
        {
            id = id.HasValue ? id.Value : 0;
            return _context.Students.Any(e => e.StudentID == id);
        }

        public bool TeacherExists(int? id)
        {
            id = id.HasValue ? id.Value : 0;
            return _context.Teachers.Any(e => e.TeacherID == id);
        }

        public bool GradeExists(int? id)
        {
            id = id.HasValue ? id.Value : 0;
            return _context.Grades.Any(e => e.GradeID == id);
        }

        public bool HeadTeacherExists(int? id)
        {
            id = id.HasValue ? id.Value : 0; // id = 0 -> teacher not exist
            return _context.Teachers.Any(e => e.TeacherID == id);
        }

        public bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.ClassID == id);
        }

        // Check for unique index (Name, Year)
        public bool ClassExists(string name, int year)
        {
            var aClass =
                    from c in _context.Classes
                    where c.Name == name && c.Year == year
                    select c;

            return aClass.Any();
        }
    }
}
