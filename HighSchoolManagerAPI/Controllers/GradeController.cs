using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HighSchoolManagerAPI.Data;
using HighSchoolManagerAPI.Models;
using HighSchoolManagerAPI.FrontEndModels;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Manager")]
    public class GradeController : ControllerBase
    {
        private readonly HighSchoolContext _context;
        public GradeController(HighSchoolContext context)
        {
            _context = context;
        }

        //Create new grade 
        [HttpPost("Create")]
        public async Task<ActionResult> CreateGrade(GradeModel model)
        {
            if (ModelState.IsValid)
            {
                Grade grade = new Grade
                {
                    Name = model.Name
                };
                await _context.Grades.AddAsync(grade);
                await _context.SaveChangesAsync();
                return StatusCode(201, grade); // 201 created
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
                return BadRequest(errors); //400 bad request
            }
        }

        //get grade list
        [HttpGet("Get")]
        public async Task<ActionResult> GetGrades(int? gradeId, string name)
        {
            // filter by gradeId
            if (gradeId != null)
            {
                var aGrade = await _context.Grades.FindAsync(gradeId);
                if (aGrade == null)
                {
                    return NotFound();
                }
                return Ok(aGrade);
            }

            // if gradeId == null
            var grades =
                from g in _context.Grades
                select g;

            // filter by name
            if (!String.IsNullOrEmpty(name))
            {
                grades = grades.Where(g => g.Name.Contains(name));
            }

            grades = grades.OrderBy(g => g.GradeID);
            return Ok(await grades.ToListAsync());
        }

        //Put Grade
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutGrade(int gradeId, GradeModel gradeModel)
        {
            var grade = await _context.Grades.FindAsync(gradeId);

            // if no student is found
            if (grade == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                grade.Name = gradeModel.Name; //Set new name
                _context.Grades.Update(grade);
                await _context.SaveChangesAsync();

                return Ok(grade);
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
        }*/

        //Delete grade
        /*[HttpDelete("{id}")]
        public async Task<ActionResult<Grade>> DeleteGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            _context.Grades.Remove(grade); // delete from database
            await _context.SaveChangesAsync();

            return Ok(grade);
        }

        // Check if grade exist  
        private bool GradeExists(int id)
        {
            return _context.Grades.Any(e => e.GradeID == id);
        }*/

    }
}