using System.Collections.Generic;
using HighSchoolManagerAPI.FrontEndModels;

namespace HighSchoolManagerAPI.Services.IServices
{
    public interface IReportService
    {
        List<ReportModel> StudentsRanking(List<ReportModel> reports);
        List<ReportModel> EvaluatePerformance(List<ReportModel> reports);
    }
}