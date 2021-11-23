using api.fakebook.extensions;
using api.fakebook.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static api.fakebook.Models.Authentication.Response;

namespace api.fakebook.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private  UserManager<ApplicationUser> _userManager { get; init; }
        private  RoleManager<IdentityRole> _roleManager { get; init; }
        private IConfiguration _configuration { get; init; }

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel register)
        {

            if (!ModelState.IsValid) { return BadRequest(BadAccountCreation()); }

            if (await _userManager.FindByNameAsync(register.Username) != null)
                return BadRequest(BadAccountCreation()); ;

            ApplicationUser user = new()
            {
                UserName = register.Username,
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString()           
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
            
                return BadRequest(BadAccountCreation().IdentityErrors(result.Errors));
            }
               
            return Ok(new Response().Ok().Message(ResponseMessages.ACCOUNT_CREATION_OK));
        }


        private Response BadAccountCreation()
        {
            return new Response().Status(ResponseCodes.ERROR).Message(ResponseMessages.ACCOUNT_CREATION_ERROR);
        }



    }
}
