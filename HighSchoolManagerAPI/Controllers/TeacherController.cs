using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HighSchoolManagerAPI.Data;
using HighSchoolManagerAPI.Models;
using HighSchoolManagerAPI.FrontEndModels;
using Microsoft.AspNetCore.Authorization;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly HighSchoolContext _context;

        public TeacherController(HighSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Teacher/Get
        [HttpGet("Get")]
        public async Task<ActionResult> GetTeachers(int? teacherId, string name, string gender)
        {
            // filter by teacherId
            if (teacherId != null)
            {
                var aTeacher = await _context.Teachers.FindAsync(teacherId);
                if (aTeacher == null)
                {
                    return NotFound();
                }
                return Ok(aTeacher);
            }

            // if studentId == null

            var teachers =
                from t in _context.Teachers
                select t;

            // filter by name
            if (!String.IsNullOrEmpty(name))
            {
                teachers = teachers.Where(t => t.Name.Contains(name));
            }

            // filter by gender
            if (!String.IsNullOrEmpty(gender))
            {
                teachers = teachers.Where(t => t.Gender.Equals(gender));
            }

            teachers = teachers.OrderBy(t => t.TeacherID);

            return Ok(await teachers.ToListAsync());
        }

        // PUT: api/Teacher/Edit?teacherId=5
        [HttpPut("Edit")]
        [Authorize(Roles = "Manager, Teacher")]
        public async Task<ActionResult> EditTeacher(int teacherId, TeacherModel model)
        {
            var teacher = await _context.Teachers.FindAsync(teacherId);

            // if no teacher is found
            if (teacher == null)
            {
                return NotFound();
            }

            // check if model matches with data annotation in front-end model
            if (ModelState.IsValid)
            {
                //bind value
                teacher.Name = model.Name;
                teacher.Gender = model.Gender;
                teacher.Birthday = model.Birthday;

                // Update in DbSet
                _context.Teachers.Update(teacher);

                // Save changes in database
                await _context.SaveChangesAsync();

                return Ok(teacher);
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

        // POST: api/Teacher/Create
        [HttpPost("Create")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> CreateTeacher(TeacherModel model)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = new Teacher
                {
                    Name = model.Name,
                    Gender = model.Gender,
                    Birthday = model.Birthday
                };

                await _context.Teachers.AddAsync(teacher);
                await _context.SaveChangesAsync();
                return StatusCode(201, teacher); // 201: Created
            }
            else
            {
                // response helper method
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

        // DELETE: api/Teacher/Delete?teacherId=5
        [HttpDelete("Delete")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteTeacher(int teacherId)
        {
            var teacher = await _context.Teachers.FindAsync(teacherId);

            // if no teacher is found
            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return Ok(teacher);
        }
    }
}
