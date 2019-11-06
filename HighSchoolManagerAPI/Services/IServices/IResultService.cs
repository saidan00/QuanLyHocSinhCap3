using System.Collections.Generic;
using ApplicationCore.Entities;

namespace HighSchoolManagerAPI.Services.IServices
{
    public interface IResultService
    {
        Result GetResult(int resultId);
        Result GetResult(int studentId, int subjectId, int semesterId, int month);
        IEnumerable<Result> GetResults(int? resultId, int? studentId, int? semesterId, int? subjectId, string sort);
        void CreateResult(Result result);
        ResultDetail GetResultDetail(int resultId, int resultTypeId, int month);
        IEnumerable<ResultDetail> GetResultDetails(int resultId, int month);
        void CreateDetail(ResultDetail detail);
        void Update();
    }
}