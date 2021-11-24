using api.fakebook.extensions;
using api.fakebook.Models.Authentication;
using api.fakebook.Services.AuthService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static api.fakebook.Models.Authentication.Response;

namespace api.fakebook.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private  IAuthService _authService { get; init; }

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel register)
        {

            if (!ModelState.IsValid) { return BadRequest(BadAccountCreation()); }

            if (await _authService.FindUserByNameAsync(register.Username) != null)
                return BadRequest(BadAccountCreation()); ;

            ApplicationUser user = new()
            {
                UserName = register.Username,
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString()           
            };

            var result = await _authService.CreateUserAsync(user, register.Password);

            if (!result.Succeeded)
            {
                return BadRequest(BadAccountCreation().IdentityErrors(result.Errors));
            }
               
            return Ok(new Response().Ok().Message(ResponseMessages.ACCOUNT_CREATION_OK));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _authService.FindUserByNameAsync(login.Username);

            if (user == null) return Unauthorized();

            if (!await _authService.CheckUserPasswordAsync(user, login.Password)) return Unauthorized();

            var token = await _authService.GenerateJwtToken(user);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo});
        }


        private Response BadAccountCreation()
        {
            return new Response().Status(ResponseCodes.ERROR).Message(ResponseMessages.ACCOUNT_CREATION_ERROR);
        }



    }
}
