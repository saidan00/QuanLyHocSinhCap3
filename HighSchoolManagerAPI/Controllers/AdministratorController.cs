using Microsoft.AspNetCore.Mvc;
using HighSchoolManagerAPI.FrontEndModels;
using HighSchoolManagerAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdministratorController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private ResponeHelper resp;
        public AdministratorController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            resp = new ResponeHelper();
        }

        // Get all roles
        [HttpGet("Roles")]
        public IEnumerable<IdentityRole> GetRoles()
        {
            var roles = roleManager.Roles;
            return roles;
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult> CreateRole(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return StatusCode(200);
                }
                else
                {
                    var errors = result.Errors;
                    return BadRequest(errors);
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

        [HttpPost("AddUserToRole")]
        public async Task<ActionResult> AddUserToRole(string UserName, string RoleName)
        {
            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                resp.status = 404;
                resp.errors.Add("User " + UserName + " not found");
                return NotFound(resp);
            }

            var role = await roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                resp.status = 404;
                resp.errors.Add("Role " + RoleName + " not found");
                return NotFound(resp);
            }

            // if user is not in role
            if (!(await userManager.IsInRoleAsync(user, RoleName)))
            {
                IdentityResult result = await userManager.AddToRoleAsync(user, RoleName);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    var errors = result.Errors;
                    return BadRequest(errors);
                }
            }
            else
            {
                resp.status = 400;
                resp.errors.Add("User " + UserName + " is already in role " + RoleName);
                return BadRequest(resp);
            }
        }

        [HttpPost("RemoveUserFromRole")]
        public async Task<ActionResult> RemoveUserFromRole(string UserName, string RoleName)
        {
            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                resp.status = 404;
                resp.errors.Add("User " + UserName + " not found");
                return NotFound(resp);
            }

            var role = await roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                resp.status = 404;
                resp.errors.Add("Role " + RoleName + " not found");
                return NotFound(resp);
            }

            // if user is in role
            if (await userManager.IsInRoleAsync(user, RoleName))
            {
                IdentityResult result = await userManager.RemoveFromRoleAsync(user, RoleName);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    var errors = result.Errors;
                    return BadRequest(errors);
                }
            }
            else
            {
                resp.status = 400;
                resp.errors.Add("User " + UserName + " is not in role " + RoleName);
                return BadRequest(resp);
            }
        }
    }
}