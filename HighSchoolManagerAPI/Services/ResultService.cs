using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HighSchoolManagerAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace HighSchoolManagerAPI.Services
{
    public class ResultService : IResultService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResultService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result GetResult(int resultId)
        {
            var result = _unitOfWork.Result.GetBy(resultId);

            return result;
        }

        public Result GetResult(int studentId, int subjectId, int semesterId, int month)
        {
            var result = _unitOfWork.Result.GetAll();
            result = result.Where(r => r.StudentID == studentId);
            result = result.Where(r => r.SubjectID == subjectId);
            result = result.Where(r => r.SemesterID == semesterId);

            result = result
                    .IncludeFilter(r => r.ResultDetails.Where(d => d.Month == month).Select(d => d.ResultType));


            return result.FirstOrDefault();
        }

        public IEnumerable<Result> GetResultsWithYear(int studentId, int subjectId, int year, int? resultTypeId)
        {
            var results = _unitOfWork.Result.GetAll();
            results = results.Where(r => r.StudentID == studentId);
            results = results.Where(r => r.SubjectID == subjectId);
            results = results.Where(r => r.Semester.Year == year);

            results = results.Include(r => r.Semester);

            if (resultTypeId != null)
            {
                results = results.IncludeFilter(r => r.ResultDetails.Where(d => d.ResultTypeID == resultTypeId).Select(d => d.ResultType));
            }
            else
            {
                results = results.IncludeFilter(r => r.ResultDetails.Select(d => d.ResultType));
            }

            return results;
        }

        public IEnumerable<Result> GetResults(int? studentId, int? semesterId, int? subjectId, string sort)
        {
            var results = _unitOfWork.Result.GetAll();

            // filter by stundentId
            if (studentId != null)
            {
                results = results.Where(r => r.StudentID == studentId);
            }

            // filter by semestertId
            if (semesterId != null)
            {
                results = results.Where(r => r.SemesterID == semesterId);
            }

            // filter by subjectId
            if (subjectId != null)
            {
                results = results.Where(r => r.SubjectID == subjectId);
            }

            // order by ResultID
            results = results.OrderBy(r => r.ResultID);
            // order by others
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "student":
                        results = results.OrderBy(r => r.StudentID);
                        break;
                    case "student-desc":
                        results = results.OrderByDescending(r => r.StudentID);
                        break;
                    case "semester":
                        results = results.OrderBy(r => r.SemesterID);
                        break;
                    case "semester-desc":
                        results = results.OrderByDescending(r => r.SemesterID);
                        break;
                    case "subject":
                        results = results.OrderBy(r => r.SubjectID);
                        break;
                    case "subject-desc":
                        results = results.OrderByDescending(r => r.SubjectID);
                        break;
                    default:
                        break;
                }
            }

            results = results.Include(r => r.ResultDetails).ThenInclude(d => d.ResultType);

            return results;
        }

        public void CreateResult(Result result)
        {
            _unitOfWork.Result.Add(result);
            _unitOfWork.Complete();
        }

        public ResultDetail GetResultDetail(int resultId, int resultTypeId, int month)
        {
            return _unitOfWork.Result.GetResultDetail(resultId, resultTypeId, month);
        }

        public IEnumerable<ResultDetail> GetResultDetails(int resultId, int month)
        {
            return _unitOfWork.Result.GetResultDetails(resultId, month);
        }

        public void CreateDetail(ResultDetail detail)
        {
            _unitOfWork.Result.AddDetail(detail);
            _unitOfWork.Complete();
        }

        public void Update()
        {
            _unitOfWork.Complete();
        }

        public void CreateResultDetails(Result result, int month)
        {
            var resultTypes = this.GetAllResultTypes();
            foreach (var t in resultTypes)
            {
                if (t.Coefficient != 3)
                {
                    var detail = new ResultDetail
                    {
                        ResultID = result.ResultID,
                        ResultTypeID = t.ResultTypeID,
                        Month = month
                    };
                    _unitOfWork.Result.AddDetail(detail);
                }
                else
                {
                    if (month == 12 || month == 5)
                    {
                        var detail = new ResultDetail
                        {
                            ResultID = result.ResultID,
                            ResultTypeID = t.ResultTypeID,
                            Month = month
                        };
                        _unitOfWork.Result.AddDetail(detail);
                    }
                }
            }
            _unitOfWork.Complete();
        }

        public IEnumerable<ResultType> GetAllResultTypes()
        {
            return _unitOfWork.Result.GetAllResultTypes();
        }
    }
}