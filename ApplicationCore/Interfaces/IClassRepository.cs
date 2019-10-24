using System.Collections.Generic;
using ApplicationCore.Entities;
namespace ApplicationCore.Interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
        Class GetByNameAndYear(string name, int year);
        Grade GetGrade(int gradeId);
        IEnumerable<Grade> GetGrades(string name);
    }
}
