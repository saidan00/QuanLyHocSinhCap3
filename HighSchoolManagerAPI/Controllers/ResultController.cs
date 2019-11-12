using Microsoft.AspNetCore.Mvc;
using HighSchoolManagerAPI.Services.IServices;
using HighSchoolManagerAPI.FrontEndModels;
using System.Collections.Generic;
using ApplicationCore.Entities;
using HighSchoolManagerAPI.Helpers;
using System.Linq;
using System;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Manager, Teacher")]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;
        private readonly IExistHelper _exist;
        private readonly ResponseHelper resp;
        public ResultController(IResultService resultService, IExistHelper exist)
        {
            _resultService = resultService;
            _exist = exist;
            resp = new ResponseHelper();
        }

        // GET: api/Result/Get
        [HttpGet("Get")]
        public ActionResult GetResults(int? resultId, int? studentId, int? semesterId, int? subjectId, string sort)
        {
            // filter by resultId
            if (resultId != null)
            {
                var result = _resultService.GetResult((int)resultId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }

            // if resultId == null
            var results = _resultService.GetResults(resultId, studentId, semesterId, subjectId, sort);

            return Ok(results);
        }

        [HttpPost("UpdateMark")]
        public ActionResult UpdateMark(UpdateMarkModel model)
        {
            if (ModelState.IsValid)
            {
                if (!IsKeyValid(model))
                {
                    return StatusCode(resp.code, resp);
                }
                else
                {
                    var result = _resultService.GetResult(model.StudentID, model.SubjectID, model.SemesterID, model.Month);

                    // Create new result if not exist
                    if (result == null)
                    {
                        result = new Result
                        {
                            StudentID = model.StudentID,
                            SemesterID = model.SemesterID,
                            SubjectID = model.SubjectID
                        };

                        _resultService.CreateResult(result);

                        // get again for included entities
                        result = _resultService.GetResults(null, model.StudentID, model.SemesterID, model.SubjectID, null)
                                .AsQueryable()
                                .FirstOrDefault();
                    }

                    // details
                    var detail = _resultService.GetResultDetail(result.ResultID, model.ResultTypeID, model.Month);

                    if (detail == null)
                    {
                        if (result.Semester.Label.Equals("1"))
                        {
                            if (!(model.Month >= 8 && model.Month <= 12))
                            {
                                resp.code = 400; // Bad Request
                                resp.messages.Add(new { Month = "Month " + model.Month + " - Semester " + result.Semester.Label + " is invalid" });
                                return BadRequest(resp);
                            }
                        }
                        else
                        {
                            if (!(model.Month >= 1 && model.Month <= 5))
                            {
                                resp.code = 400; // Bad Request
                                resp.messages.Add(new { Month = "Month " + model.Month + " - Semester " + result.Semester.Label + " is invalid" });
                                return BadRequest(resp);
                            }
                        }

                        _resultService.CreateResultDetails(result, model.Month);
                        detail = _resultService.GetResultDetail(result.ResultID, model.ResultTypeID, model.Month);
                    }

                    detail.Mark = model.Mark;
                    _resultService.Update();

                    CalculateSubjectMonthlyAverage(result);

                    return Ok(detail);
                }
            }
            else
            {
                var errors = new List<string>();
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(errors);
            }
        }

        // GET: api/Result/SubjectMonthlyAverage?studentId=1&subjectId=1&semesterId=1&month=1
        [HttpGet("SubjectMonthlyAverage")]
        public ActionResult SubjectMonthlyAverage(int studentId, int subjectId, int semesterId, int month)
        {
            var result = _resultService.GetResult(studentId, subjectId, semesterId, month);

            if (result == null)
            {
                double? avg_temp = null;
                Object subjectMonthlyResult_temp = new
                {
                    average = avg_temp
                };
                return Ok(avg_temp);
            }

            Object subjectMonthlyResult = new
            {
                studentId = result.StudentID,
                subjectId = result.SubjectID,
                semesterId = result.SemesterID,
                resultDetails = result.ResultDetails
            };

            return Ok(subjectMonthlyResult);
        }

        [NonAction]
        public double? CalculateSubjectMonthlyAverage(Result result)
        {
            var markColumns = 2; // số cột điểm 
            double? avg = 0;
            if (result.ResultDetails.Count(d => d.Mark != null) == markColumns)
            {
                double sum = 0;
                double coefficients = 0;

                foreach (var d in result.ResultDetails)
                {
                    sum += (double)d.Mark * d.ResultType.Coefficient;
                    coefficients += d.ResultType.Coefficient;
                }

                avg = Math.Round(sum / coefficients, 1);
            }
            else
            {
                avg = null;
            }

            // save to db base
            foreach (var d in result.ResultDetails)
            {
                d.MonthlyAverage = avg;
            }
            _resultService.Update();

            return avg;
        }

        [HttpGet("SubjectMonthlyAverages")]
        public ActionResult SubjectMonthlyAverages(int studentId, int subjectId, int year)
        {
            var results = _resultService.GetResults(studentId, subjectId, year);

            List<object> details = new List<object>();

            foreach (var r in results)
            {
                foreach (var d in r.ResultDetails)
                {
                    details.Add(new
                    {
                        month = d.Month,
                        average = d.MonthlyAverage
                    });
                }
            }


            object subjectMonthlyAverages = new
            {
                studentId = studentId,
                subjectId = subjectId,
                year = year,
                averages = details
            };

            return Ok(subjectMonthlyAverages);
        }

        private bool IsKeyValid(UpdateMarkModel model)
        {
            // check for student
            if (!_exist.StudentExists(model.StudentID))
            {
                resp.code = 404; // Not found
                resp.messages.Add("Student not found");
            }

            // check for subject
            if (!_exist.SubjectExists(model.SubjectID))
            {
                resp.code = 404; // Not found
                resp.messages.Add("Subject not found");
            }

            // check for semester
            if (!_exist.SemesterExists(model.SemesterID))
            {
                resp.code = 404; // Not found
                resp.messages.Add("Semester not found");
            }

            // check for result type
            if (!_exist.ResultTypeExists(model.ResultTypeID))
            {
                resp.code = 404; // Not found
                resp.messages.Add("Result Type not found");
            }

            return true;
        }
    }

}