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
                    var result = _resultService.GetResults(null, model.StudentID, model.SemesterID, model.SubjectID, null)
                            .AsQueryable()
                            .FirstOrDefault();

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
                    }

                    var detail = _resultService.GetResultDetail(result.ResultID, model.ResultTypeID, model.Month);

                    if (detail == null)
                    {
                        detail = new ResultDetail
                        {
                            ResultID = result.ResultID,
                            ResultTypeID = model.ResultTypeID,
                            Month = model.Month
                        };

                        _resultService.CreateDetail(detail);
                    }

                    detail.Mark = model.Mark;
                    _resultService.Update();

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

        // GET: api/Result/CalculateSubjectAverage?studentId=1&subjectId=1&semesterId=1&month=1
        [HttpGet("SubjectMonthlyAverage")]
        public ActionResult SubjectMonthlyAverage(int studentId, int subjectId, int semesterId, int month)
        {
            var result = _resultService.GetResult(studentId, subjectId, semesterId, month);

            if (result == null)
            {
                return NotFound();
            }

            var avg = CalculateSubjectMonthlyAverage(result);

            // result.Average = avg;
            // _resultService.Update();
            // vì đây là điểm tháng -> không lưu db

            Object subjectMonthlyResult = new
            {
                studentId = result.StudentID,
                subjectId = result.SubjectID,
                semesterId = result.SemesterID,
                resultDetails = result.ResultDetails,
                avarage = avg
            };

            return Ok(subjectMonthlyResult);
        }

        public double? CalculateSubjectMonthlyAverage(Result result)
        {
            var markColumns = 2; // số cột điểm 
            double? avg = 0;
            if (result.ResultDetails.Count() == markColumns)
            {
                double sum = 0;
                double coefficients = 0;
                foreach (var d in result.ResultDetails)
                {
                    sum += d.Mark * d.ResultType.Coefficient;
                    coefficients += d.ResultType.Coefficient;
                }
                avg = sum / coefficients;
            }
            else
            {
                avg = null;
            }

            return avg;
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