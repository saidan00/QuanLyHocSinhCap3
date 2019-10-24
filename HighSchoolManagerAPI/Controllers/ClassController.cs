using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Entities;
using Infrastructure.Persistence;
using HighSchoolManagerAPI.FrontEndModels;
using HighSchoolManagerAPI.Helpers;
using Microsoft.Data.SqlClient;
using HighSchoolManagerAPI.Services;
using ApplicationCore.Interfaces;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private ResponseHelper resp;
        private IExistHelper exist;

        public ClassController(IClassService classService, IExistHelper exist)
        {
            _classService = classService;
            this.resp = new ResponseHelper();
            this.exist = exist;
        }

        // GET: api/Class/Get -> Get all classes
        // GET: api/Class/Get?classId=2
        [HttpGet("Get")]
        public ActionResult GetClasses(int? classId, string name, int? year, int? gradeId, int? headTeacherId, string sort)
        {
            // filter by classId
            if (classId != null)
            {
                var aClass = _classService.GetClass((int)classId);
                if (aClass == null)
                {
                    return NotFound();
                }
                return Ok(aClass);
            }
            // if classId == null
            var classes = _classService.GetClasses(classId, name, year, gradeId, headTeacherId, sort);

            return Ok(classes);
        }

        // PUT: api/Class/Edit?classId=5
        [HttpPut("Edit")]
        public IActionResult EditClass(int classId, ClassModel model)
        {
            var aClass = _classService.GetClass(classId);

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

                _classService.Update();

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
        public ActionResult CreateClass(ClassModel model)
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

                _classService.CreateClass(aClass);

                Console.WriteLine(aClass.Name);
                Console.WriteLine(aClass.Year);
                Console.WriteLine(aClass.Size);
                Console.WriteLine(aClass.GradeID);
                Console.WriteLine(aClass.HeadTeacherID);

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
                if (!exist.TeacherExists(model.HeadTeacherID))
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

        // should not use delete yet, maybe later
        // DELETE: api/Class/Delete?classId=5
        // [HttpDelete("Delete")]
        [NonAction]
        public ActionResult DeleteClass(int classId)
        {
            var aClass = _classService.GetClass(classId);

            // if no class is found
            if (aClass == null)
            {
                return NotFound();
            }

            _classService.DeleteClass(aClass);

            return Ok(aClass);
        }


        // PUT: aspi/Class/AddStudentsToClass?classId=5
        // JSON: {"studentIds" : [1, 2, 3, 4]} (array of student id)
        // [HttpPut("AddStudentsToClass")]
        // public async Task<ActionResult> AddStudentsToClass(int classId, IdListModel model)
        // {
        //     // if class exist
        //     if (exist.ClassExists(classId))
        //     {
        //         foreach (var id in model.ids)
        //         {
        //             Student student = await _context.Students.FindAsync(id);

        //             // check if student exist
        //             if (student == null)
        //             {
        //                 resp.code = 404; // Not found
        //                 resp.messages.Add(new { studentID = "Student ID " + id + " not found" });

        //                 return NotFound(resp);
        //             }
        //             else
        //             {
        //                 // bind value(s)
        //                 student.ClassID = classId;

        //                 _context.Students.Update(student);
        //             }
        //         }

        //         await _context.SaveChangesAsync();

        //         return Ok();
        //     }
        //     // if no class found
        //     else
        //     {
        //         resp.code = 404; // Not found
        //         resp.messages.Add(new { classID = "Class ID does not exist" });

        //         return NotFound(resp);
        //     }
        // }
    }
}
