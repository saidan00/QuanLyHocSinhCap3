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
using HighSchoolManagerAPI.Helpers;
using Microsoft.Data.SqlClient;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly HighSchoolContext _context;
        private ResponseHelper resp;
        private ExistHelper exist;

        public ClassController(HighSchoolContext context)
        {
            _context = context;
            resp = new ResponseHelper();
            exist = new ExistHelper(_context);
        }

        // GET: api/Class/Get -> Get all classes
        // GET: api/Class/Get?classId=2
        [HttpGet("Get")]
        public async Task<ActionResult> GetClasses(int? classId, string name, int? year, int? gradeId, int? headTeacherId)
        {
            // filter by classId
            if (classId != null)
            {
                var aClass = await _context.Classes.FindAsync(classId);
                if (aClass == null)
                {
                    return NotFound();
                }
                return Ok(aClass);
            }

            // if classId == null

            var classes =
                from c in _context.Classes
                select c;

            // filter by name
            if (!String.IsNullOrEmpty(name))
            {
                classes = classes.Where(c => c.Name.Contains(name));
            }

            // filter by year
            if (year != null)
            {
                classes = classes.Where(c => c.Year == year);
            }

            // filter by gradeId
            if (gradeId != null)
            {
                classes = classes.Where(c => c.GradeID == gradeId);
            }

            // filter by headTeacherId
            if (headTeacherId != null)
            {
                classes = classes.Where(c => c.HeadTeacherID == headTeacherId);
            }

            classes = classes.OrderBy(c => c.ClassID);

            return Ok(await classes.ToListAsync());
        }

        // PUT: api/Class/Edit?classId=5
        [HttpPut("Edit")]
        public async Task<IActionResult> EditClass(int classId, ClassModel model)
        {
            var aClass = await _context.Classes.FindAsync(classId);

            // if no class is found
            if (aClass == null)
            {
                return NotFound();
            }

            // check if foreign key(s), unique key(s) are invalid
            if (!IsKeysValid(model))
            {
                return StatusCode(resp.code, resp);
            }

            // check if model matches with data annotation in front-end model
            if (ModelState.IsValid)
            {
                aClass.Name = model.Name;
                aClass.Year = model.Year;
                aClass.GradeID = model.GradeID;
                aClass.HeadTeacherID = model.HeadTeacherID;

                // Update in DbSet
                _context.Classes.Update(aClass);

                // Save changes in database
                await _context.SaveChangesAsync();

                return Ok(aClass);
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

        // POST: api/Class/Create
        [HttpPost("Create")]
        public async Task<ActionResult> CreateClass(ClassModel model)
        {
            if (ModelState.IsValid)
            {
                // check if foreign key(s), unique key(s) are invalid
                if (!IsKeysValid(model))
                {
                    return StatusCode(resp.code, resp);
                }

                Class aClass = new Class
                {
                    Name = model.Name,
                    Year = model.Year,
                    GradeID = model.GradeID,
                    HeadTeacherID = model.HeadTeacherID
                };

                await _context.Classes.AddAsync(aClass);

                await _context.SaveChangesAsync();

                return StatusCode(201, aClass); // 201: Created
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

        // DELETE: api/Class/Delete?classId=5
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteClass(int classId)
        {
            var aClass = await _context.Classes.FindAsync(classId);

            // if no class is found
            if (aClass == null)
            {
                return NotFound();
            }

            _context.Classes.Remove(aClass);
            await _context.SaveChangesAsync();

            return Ok(aClass);
        }

        // PUT: aspi/Class/AddStudentsToClass?classId=5
        // JSON: {"studentIds" : [1, 2, 3, 4]} (array of student id)
        [HttpPut("AddStudentsToClass")]
        public async Task<ActionResult> AddStudentsToClass(int classId, IdListModel model)
        {
            // if class exist
            if (exist.ClassExists(classId))
            {
                foreach (var id in model.ids)
                {
                    Student student = await _context.Students.FindAsync(id);

                    // check if student exist
                    if (student == null)
                    {
                        resp.code = 404; // Not found
                        resp.messages.Add(new { studentID = "Student ID " + id + " not found" });

                        return NotFound(resp);
                    }
                    else
                    {
                        // bind value(s)
                        student.ClassID = classId;

                        _context.Students.Update(student);
                    }
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            // if no class found
            else
            {
                resp.code = 404; // Not found
                resp.messages.Add(new { classID = "Class ID does not exist" });

                return NotFound(resp);
            }
        }

        private bool IsKeysValid(ClassModel model)
        {
            // Check if Grade ID exists
            if (!exist.GradeExists(model.GradeID))
            {
                resp.code = 404;
                resp.messages.Add(new { GradeID = "Grade not found" });
                return false;
            }

            // check if Head Teacher exists
            if (model.HeadTeacherID != null)
            {
                if (!exist.HeadTeacherExists(model.HeadTeacherID))
                {
                    resp.code = 404;
                    resp.messages.Add(new { HeadTeacherID = "Head teacher not found" });
                    return false;
                }
            }

            // Check for unique index (Name, Year)
            if (exist.ClassExists(model.Name, model.Year))
            {
                resp.code = 400; // bad request
                resp.messages.Add(new { Name = "Already have class " + model.Name + " in " + model.Year });
                return false;
            }

            // all keys are valid
            return true;
        }

        /* NOT DONE YET */
        // GET: api/Class/NumberOfPossibleClasses?grade=10&year=2019
        // [HttpGet("NumberOfPossibleClasses")]
        // public async Task<ActionResult> NumberOfPossibleClasses(string grade, int year)
        // {
        //     int classMaxSize = 40;
        //     int numberOfClasses = 0;
        //     int numberOfStudentWithoutClass = 0;

        //     // select student(s) that belong to <grade> and did not have class (classID = null)
        //     var students =
        //             from s in _context.Students
        //             where s.Class.Grade.Name == grade && s.ClassID == null
        //             select s;

        //     // Count number of student(s)
        //     numberOfStudentWithoutClass = await students.CountAsync();

        //     // Calculate number of class(es)
        //     // exp: 81 students -> 81 / 40 = 2 -> 2 + 1 = 3 (classes)
        //     numberOfClasses = (numberOfStudentWithoutClass / classMaxSize) + 1;

        //     return Ok(new { classes = numberOfClasses }); // exp: {"classes": 3}, Ok: 200
        // }
    }
}
