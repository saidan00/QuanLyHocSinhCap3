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
using HighSchoolManagerAPI.Helpers;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager, Teacher")] // giáo vụ, giáo viên
    public class StudentController : ControllerBase
    {
        private readonly HighSchoolContext _context;
        private ResponseHelper resp;

        public StudentController(HighSchoolContext context)
        {
            _context = context;
            resp = new ResponseHelper();
        }

        // Return a student or not found if filter by studentId
        // Return a list of student or empty list if filter by others
        // Get students that are not in class -> classId = 0
        [HttpGet("Get")]
        public async Task<ActionResult> GetStudents(int? studentId, string name, int? gradeId, int? classId, int? year, string sort)
        {
            // filter by studentId
            if (studentId != null)
            {
                var aStudent = await _context.Students.FindAsync(studentId);
                if (aStudent == null)
                {
                    return NotFound();
                }
                return Ok(aStudent);
            }

            // if studentId == null

            var students =
                from s in _context.Students
                select s;

            // filter by first name or last name
            if (!String.IsNullOrEmpty(name))
            {
                List<string> namesArr = name.Split(' ').ToList();
                var newStudents = students.Where(s => s.FirstName.Contains(namesArr[0]) || s.LastName.Contains(namesArr[0]));
                for (int i=1; i<namesArr.Count; i++) {
                    string newName = namesArr[i];
                    var newStudents2 = students.Where(s => s.FirstName.Contains(newName) || s.LastName.Contains(newName));
                    newStudents = newStudents.Union(newStudents2);
                }
                students = newStudents;
            }

            // filter by gradeId
            if (gradeId != null)
            {
                students = students.Where(s => s.Class.GradeID == gradeId);
            }

            // filter by classId
            if (classId != null)
            {
                if (classId == 0)
                {
                    students = students.Where(s => s.ClassID == null);
                }
                else
                {
                    students = students.Where(s => s.ClassID == classId);
                }
            }

            // filter by year (Class.year)
            if (year != null)
            {
                students = students.Where(s => s.Class.Year == year);
            }

            // order by studentId (default)
            students = students.OrderBy(s => s.StudentID);

            // order by others
            if (!String.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "firstname":
                        students = students.OrderBy(s => s.FirstName);
                        break;
                    case "firstname-desc":
                        students = students.OrderByDescending(s => s.FirstName);
                        break;
                    case "lastname":
                        students = students.OrderBy(s => s.LastName);
                        break;
                    case "lastname-desc":
                        students = students.OrderByDescending(s => s.LastName);
                        break;
                    case "class":
                        students = students.OrderBy(s => s.ClassID);
                        break;
                    case "class-desc":
                        students = students.OrderByDescending(s => s.ClassID);
                        break;
                    case "grade":
                        students = students.OrderBy(s => s.Class.GradeID);
                        break;
                    case "grade-desc":
                        students = students.OrderByDescending(s => s.Class.GradeID);
                        break;
                    case "year":
                        students = students.OrderBy(s => s.Class.Year);
                        break;
                    case "year-desc":
                        students = students.OrderByDescending(s => s.Class.Year);
                        break;
                    default:
                        break;
                }
            }

            return Ok(await students.ToListAsync());
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> CreateStudent(StudentModel model)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Birthday = model.Birthday,
                    Address = model.Address,
                    ClassID = model.ClassID
                };

                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
                return StatusCode(201, student); // 201: Created
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

        [HttpPut("Edit")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> EditStudent(int studentId, StudentModel model)
        {
            var student = await _context.Students.FindAsync(studentId);

            // if no student is found
            if (student == null)
            {
                return NotFound();
            }

            // check if model matches with data annotation in front-end model
            if (ModelState.IsValid)
            {
                //bind value
                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.Birthday = model.Birthday;
                student.Address = model.Address;
                student.ClassID = model.ClassID;

                // Update in DbSet
                _context.Students.Update(student);

                // Save changes in database
                await _context.SaveChangesAsync();

                return Ok(student);
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

        [HttpDelete("Delete")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Delete(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);

            // if no student is found
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }
    }
}
