using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore.Infrastructure.Models;

namespace NetCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            string role = "user";
            var identityRole = new IdentityRole(role);
            if (result.Succeeded)
            {
                if (await _roleManager.FindByNameAsync(role) == null)
                {
                    await _roleManager.CreateAsync(identityRole);
                }

                await _userManager.AddToRoleAsync(user, role);
                //await _roleManager.AddClaimAsync(identityRole, new Claim("role", role));
                await _userManager.AddClaimAsync(user, new Claim("userName", user.UserName));
                await _userManager.AddClaimAsync(user, new Claim("email", user.Email));
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));

                return Ok(new ProfileViewModel(user));
            }

            return BadRequest(result.Errors);


        }
    }
}