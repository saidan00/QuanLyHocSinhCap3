using System.Collections.Generic;
using ApplicationCore.Entities;

namespace HighSchoolManagerAPI.Services
{
    public interface IStudentService
    {
        public Student GetStudent(int studentId);
        public IEnumerable<Student> GetStudents(string name, int? gradeId, int? classId, int? year, string sort);
        public void CreateStudent(Student student);
        public void Update();
        public void DeleteStudent(Student student);
    }
}