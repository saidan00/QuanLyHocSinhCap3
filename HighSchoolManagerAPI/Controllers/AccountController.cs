using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HighSchoolManagerAPI.FrontEndModels;
using HighSchoolManagerAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private ResponeHelper resp;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            resp = new ResponeHelper();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
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
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, false);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                resp.status = 401;
                resp.errors.Add("User name or password is incorrect");
                return Unauthorized(resp);
            }
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
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

            var user = new IdentityUser { UserName = model.UserName };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // await signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            else
            {
                var errors = result.Errors;
                resp.status = 400;
                resp.errors.AddRange(errors);
                return BadRequest(resp);
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
            resp.status = 401;
            resp.errors.Add("User is not logged in");
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
            IdentityUser user = await userManager.FindByNameAsync(UserName);
            return await userManager.GetRolesAsync(user);
        }
    }
}