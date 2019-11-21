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
                int numOfSubject = subjects.Count();
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

                // if no student found
                if (students.Count() == 0)
                {
                    return Ok(monthlyReports);
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
                            // get monthly average
                            average = result.ResultDetails.ToList()[0].MonthlyAverage;
                        }

                        ResultAverage resultAverage = new ResultAverage()
                        {
                            subject = new Subject(subject),
                            average = average
                        };

                        monthlyReport.resultAvgs.Add(resultAverage);
                    }

                    monthlyReport.sumAverage = _resultService.CalculateStudentAverage(monthlyReport.resultAvgs, numOfSubject);

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

        [HttpGet("SemesterReport")]
        public ActionResult SemesterReport(int? classId, int? gradeId, int semesterId)
        {
            // object to return
            List<ReportModel> semesterReports = new List<ReportModel>();

            if (classId != null || gradeId != null)
            {
                var subjects = _subjectService.GetSubjects(null, null);
                int numOfSubject = subjects.Count();
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

                // if no student found
                if (students.Count() == 0)
                {
                    return Ok(semesterReports);
                }

                Result result = new Result();

                // calculate average for each student
                foreach (var student in students)
                {
                    ReportModel semesterReport = new ReportModel();
                    semesterReport.resultAvgs = new List<ResultAverage>();

                    semesterReport.student = new Student(student);
                    double? average = null;

                    // calculate each subject average
                    foreach (var subject in subjects)
                    {
                        // get result, filter detail by month
                        result = _resultService.GetResults(student.StudentID, semesterId, subject.SubjectID, null).FirstOrDefault();

                        if (result == null)
                        {
                            average = null;
                        }
                        else
                        {
                            // get monthly average
                            average = result.Average;
                        }

                        ResultAverage resultAverage = new ResultAverage()
                        {
                            subject = new Subject(subject),
                            average = average
                        };

                        semesterReport.resultAvgs.Add(resultAverage);
                    }

                    semesterReport.sumAverage = _resultService.CalculateStudentAverage(semesterReport.resultAvgs, numOfSubject);

                    semesterReports.Add(semesterReport);
                }

                // sort by sumAverage
                semesterReports = semesterReports.OrderByDescending(m => m.sumAverage).ToList();

                // ranking
                semesterReports = _reportService.StudentsRanking(semesterReports);

                // performance
                semesterReports = _reportService.EvaluatePerformance(semesterReports);

                return Ok(semesterReports);
            }

            return Ok(semesterReports);
        }
    }
}
