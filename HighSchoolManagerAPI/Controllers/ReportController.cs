using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Entities;
using HighSchoolManagerAPI.FrontEndModels;
using HighSchoolManagerAPI.Helpers;
using HighSchoolManagerAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IResultService _resultService;
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;
        private readonly ISemesterService _semesterService;
        private readonly IReportService _reportService;
        private readonly ResponseHelper resp;

        public ReportController(IResultService resultService, IStudentService studentService, ISubjectService subjectService, ISemesterService semesterService, IReportService reportService, IExistHelper exist)
        {
            _resultService = resultService;
            _studentService = studentService;
            _subjectService = subjectService;
            _semesterService = semesterService;
            _reportService = reportService;
            resp = new ResponseHelper();
        }

        [HttpGet("MonthlyReport")]
        public ActionResult MonthlyReport(int? classId, int? gradeId, int year, int month)
        {
            // object to return
            List<ReportModel> monthlyReports = new List<ReportModel>();

            if (classId != null || gradeId != null)
            {
                var subjects = _subjectService.GetSubjects(null, null);
                List<Student> students;
                if (classId != null)
                {
                    // get students by classid
                    students = _studentService.GetStudents(null, null, classId, null, null).ToList();
                }
                else
                {
                    // get students by gradeid
                    students = _studentService.GetStudents(null, gradeId, null, null, null).ToList();
                }

                Semester semester = new Semester();
                Result result = new Result();

                // calculate average for each student
                foreach (var student in students)
                {
                    ReportModel monthlyReport = new ReportModel();
                    monthlyReport.resultAvgs = new List<ResultAverage>();

                    monthlyReport.student = new Student(student);
                    double? average = null;

                    // get semesterId by month
                    switch (month)
                    {
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            semester = _semesterService.GetSemesters(1, year).FirstOrDefault();
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            semester = _semesterService.GetSemesters(2, year).FirstOrDefault();
                            break;
                        default:
                            break;
                    }

                    // calculate each subject average
                    foreach (var subject in subjects)
                    {
                        // get result, filter detail by month
                        result = _resultService.GetResult(student.StudentID, subject.SubjectID, semester.SemesterID, month);

                        if (result == null)
                        {
                            average = null;
                        }
                        else
                        {
                            average = _resultService.CalculateSubjectMonthlyAverage(result);
                        }

                        ResultAverage resultAverage = new ResultAverage()
                        {
                            subject = new Subject(subject),
                            average = average
                        };

                        monthlyReport.resultAvgs.Add(resultAverage);
                    }

                    monthlyReport.sumAverage = _resultService.CalculateStudentMonthlyAverage(monthlyReport.resultAvgs);

                    monthlyReports.Add(monthlyReport);
                }

                // sort by sumAverage
                monthlyReports = monthlyReports.OrderByDescending(m => m.sumAverage).ToList();

                // ranking
                monthlyReports = _reportService.StudentsRanking(monthlyReports);

                // performance
                monthlyReports = _reportService.EvaluatePerformance(monthlyReports);

                return Ok(monthlyReports);
            }

            return Ok(monthlyReports);
        }
    }
}
