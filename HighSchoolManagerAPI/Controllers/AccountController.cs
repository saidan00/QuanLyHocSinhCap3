using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HighSchoolManagerAPI.FrontEndModels;
using HighSchoolManagerAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using HighSchoolManagerAPI.Models;
using HighSchoolManagerAPI.Data;
using System.Linq;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly HighSchoolContext _context;
        private ResponseHelper resp;
        private ExistHelper exist;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, HighSchoolContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _context = context;
            resp = new ResponseHelper();
            exist = new ExistHelper(_context);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    resp.code = 401;
                    resp.messages.Add("User name or password is incorrect");
                    return Unauthorized(resp);
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

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }

        // we do not use 'register'
        [HttpPost("Create")]
        public async Task<ActionResult> CreateUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // check if foreign key(s), unique key(s) are invalid
                if (!IsKeysValid(model))
                {
                    return StatusCode(resp.code, resp);
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    TeacherID = model.TeacherID
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // await signInManager.SignInAsync(user, isPersistent: false);
                    var defaultRole = "Teacher";
                    await userManager.AddToRoleAsync(user, defaultRole);

                    return Ok();
                }
                else
                {
                    var errors = result.Errors;
                    resp.code = 400;
                    resp.messages.AddRange(errors);
                    return BadRequest(resp);
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

        // check if user is logged in
        [HttpGet("IsSignedIn")]
        public bool IsSignedIn()
        {
            return User.Identity.IsAuthenticated;
        }

        [HttpGet("NotLoggedIn")]
        public ActionResult NotLoggedIn()
        {
            resp.code = 401;
            resp.messages.Add("User is not logged in");
            return Unauthorized(resp);
        }

        [HttpGet("AccessDenied")]
        public ActionResult AccessDenied()
        {
            return StatusCode(403);
        }

        // Get curret logged in user
        [HttpGet("CurrentUser")]
        [Authorize]
        public Object CurrentUser()
        {
            var userRoles = GetUserRoles(User.Identity.Name);
            var currentUser = new
            {
                userName = User.Identity.Name,
                roles = userRoles
            };
            return currentUser;
        }

        [HttpGet("GetUserRoles")]
        [Authorize]
        public async Task<IList<string>> GetUserRoles(string UserName)
        {
            ApplicationUser user = await userManager.FindByNameAsync(UserName);
            return await userManager.GetRolesAsync(user);
        }

        // GET: api/Account/GetUserName?teacherId=5
        [HttpGet("GetUserName")]
        // [Authorize]
        public ActionResult<string> GetUserName(int? teacherId)
        {
            if (teacherId == null)
            {
                return BadRequest();
            }

            var aAccount =
                _context.Users
                .Where(a => a.TeacherID == teacherId)
                .FirstOrDefault();

            if (aAccount == null)
            {
                return NotFound();
            }

            return Ok(aAccount.UserName);
        }

        private bool IsKeysValid(RegisterModel model)
        {
            if (!exist.TeacherExists(model.TeacherID))
            {
                resp.code = 404;
                resp.messages.Add(new { Teacher = "Teacher not found" });
                return false;
            }

            return true;
        }
    }
}