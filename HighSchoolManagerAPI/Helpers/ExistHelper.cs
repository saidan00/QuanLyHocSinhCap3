using ApplicationCore.Interfaces;

namespace HighSchoolManagerAPI.Helpers
{
    // CHECK IF ENTITY EXISTS
    public class ExistHelper : IExistHelper
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExistHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool StudentExists(int? id)
        {
            id = id.HasValue ? id.Value : 0;
            return (_unitOfWork.Student.GetBy((int)id) != null);
        }

        public bool TeacherExists(int? id)
        {
            id = id.HasValue ? id.Value : 0;
            return (_unitOfWork.Teacher.GetBy((int)id) != null);
        }

        public bool GradeExists(int? id)
        {
            id = id.HasValue ? id.Value : 0;
            return (_unitOfWork.Class.GetGrade((int)id) != null);
        }

        public bool ClassExists(int id)
        {
            return (_unitOfWork.Class.GetBy((int)id) != null);
        }

        // Check for unique index (Name, Year)
        public bool ClassExists(string name, int year)
        {
            var aClass = _unitOfWork.Class.GetByNameAndYear(name, year);

            return (aClass != null);
        }
    }
}
