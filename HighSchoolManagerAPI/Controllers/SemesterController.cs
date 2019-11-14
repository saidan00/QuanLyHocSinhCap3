using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Entities;
using HighSchoolManagerAPI.FrontEndModels;
using HighSchoolManagerAPI.Services.IServices;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        // GET: api/Semester/Get
        [HttpGet("Get")]
        public ActionResult GetSemesters(int? label, int? year)
        {
            var semesters = _semesterService.GetSemesters(label, year);

            return Ok(semesters);
        }
    }
}
