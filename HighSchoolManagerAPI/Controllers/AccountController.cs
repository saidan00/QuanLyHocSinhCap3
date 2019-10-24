using System.Collections.Generic;
using HighSchoolManagerAPI.FrontEndModels;
using HighSchoolManagerAPI.Helpers;
using HighSchoolManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Authorization;
using System;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private ResponseHelper resp;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
            resp = new ResponseHelper();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginAsync(model.UserName, model.Password, isPersistent: false, false);
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
        public async Task<ActionResult> LogoutAsync()
        {
            await _accountService.LogoutAsync();
            return Ok();
        }

        // we not use 'register' here
        [HttpPost("Create")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> CreateUserAsync(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // check if foreign key(s), unique key(s) are invalid
                // if (!IsKeysValid(model))
                // {
                //     return StatusCode(resp.code, resp);
                // }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    TeacherID = model.TeacherID
                };

                var result = await _accountService.CreateUserAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // await signInManager.SignInAsync(user, isPersistent: false);
                    var defaultRole = "Teacher";
                    await _accountService.AddUserToRoleAsync(user, defaultRole);

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
        public async Task<Object> CurrentUser()
        {
            var userRoles = await _accountService.GetUserRolesAsync(User.Identity.Name);
            var currentUser = new
            {
                userName = User.Identity.Name,
                roles = userRoles
            };
            return currentUser;
        }
    }
}