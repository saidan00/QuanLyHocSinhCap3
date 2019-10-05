using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HighSchoolManagerAPI.Models;

namespace HighSchoolManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }


        [HttpPost("Login")]
        public async Task<ActionResult> Login(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, false);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
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
                return BadRequest();
            }
            var user = new IdentityUser { UserName = model.UserName };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("isSignedIn")]
        public bool IsSignedIn()
        {
            return User.Identity.IsAuthenticated;
        }

        [HttpGet("User")]
        public string CurrentUser()
        {
            return User.Identity.Name;
        }
    }
}