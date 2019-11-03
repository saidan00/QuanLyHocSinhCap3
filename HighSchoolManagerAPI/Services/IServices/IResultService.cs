using System.Collections.Generic;
using ApplicationCore.Entities;

namespace HighSchoolManagerAPI.Services.IServices
{
    public interface IResultService
    {
        Result GetResult(int resultId);
        IEnumerable<Result> GetResults(int? resultId, int? studentId, int? semesterId, int? subjectId, string sort);
        void CreateResult(Result result);
        ResultDetail GetResultDetail(int resultId, int resultTypeId, int month);
        void CreateDetail(ResultDetail detail);
        void Update();
    }
}