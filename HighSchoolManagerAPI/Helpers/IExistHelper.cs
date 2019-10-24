namespace HighSchoolManagerAPI.Helpers
{
    public interface IExistHelper
    {
        bool StudentExists(int? id);
        bool TeacherExists(int? id);
        bool GradeExists(int? id);
        bool ClassExists(int id);
        bool ClassExists(string name, int year);
    }
}