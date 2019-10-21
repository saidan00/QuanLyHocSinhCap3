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
    public class SubjectController : ControllerBase
    {
        private readonly HighSchoolContext _context;

        public SubjectController(HighSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Subject/Get
        [HttpGet("Get")]
        public async Task<ActionResult> GetSubjects(int? subjectId, string name)
        {
            // filter by subjectId
            if (subjectId != null)
            {
                var aSubject = await _context.Subjects.FindAsync(subjectId);
                if (aSubject == null)
                {
                    return NotFound();
                }
                return Ok(aSubject);
            }

            // if subjectId == null

            var subjects =
                from t in _context.Subjects
                select t;

            // filter by name
            if (!String.IsNullOrEmpty(name))
            {
                subjects = subjects.Where(t => t.Name.Contains(name));
            }

            subjects = subjects.OrderBy(s => s.SubjectID);

            return Ok(await subjects.ToListAsync());
        }

        // PUT: api/Subject/Edit?subjectId=5
        [HttpPut("Edit")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> EditSubject(int subjectId, SubjectModel model)
        {
            var subject = await _context.Subjects.FindAsync(subjectId);

            // if no subject is found
            if (subject == null)
            {
                return NotFound();
            }

            // check if model matches with data annotation in front-end model
            if (ModelState.IsValid)
            {
                //bind value
                subject.Name = model.Name;

                // Update in DbSet
                _context.Subjects.Update(subject);

                // Save changes in database
                await _context.SaveChangesAsync();

                return Ok(subject);
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

        // POST: api/Subject/Create
        [HttpPost("Create")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> CreateSubject(SubjectModel model)
        {
            if (ModelState.IsValid)
            {
                Subject subject = new Subject
                {
                    Name = model.Name
                };

                await _context.Subjects.AddAsync(subject);
                await _context.SaveChangesAsync();
                return StatusCode(201, subject); // 201: Created
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

        // DELETE: api/Subject/Delete?subjectId=5
        [HttpDelete("Delete")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteSubject(int subjectId)
        {
            var subject = await _context.Subjects.FindAsync(subjectId);

            // if no subject is found
            if (subject == null)
            {
                return NotFound();
            }

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return Ok(subject);
        }
    }
}
