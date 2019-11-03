using System.Collections.Generic;
using ApplicationCore.Entities;

namespace HighSchoolManagerAPI.Services.IServices
{
    public interface ISemesterService
    {
        Semester GetSemester(int semesterId);
    }
}