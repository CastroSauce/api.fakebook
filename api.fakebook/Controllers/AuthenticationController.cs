using api.fakebook.extensions;
using api.fakebook.Models.Authentication;
using api.fakebook.Services.AuthService;
using api.fakebook.Services.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
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
        private  IUserService _userService { get; init; }

        public AuthenticationController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel register)
        {

            if (await _userService.FindByUsernameAsync(register.Username) != null)
                return BadRequest(BadAccountCreation());

            var result = await _userService.CreateUserAsync(register);

            if (!result.Succeeded)
            {
                var errorResponse = new RegisterResponse()
                .IdentityErrors(result.Errors)
                .BadRequest()
                .Message(ResponseMessages.ACCOUNT_LOGIN_ERROR);

                return BadRequest(errorResponse);
            }

            var response = new RegisterResponse()
                .Ok()
                .Message(ResponseMessages.ACCOUNT_LOGIN_OK);
                

            return Ok(response);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var response = await _authService.Authenticate(login);

            if (response == null)
                return BadRequest(new Response().BadRequest().Message("Username of Password wrong"));

            return Ok(response);
        }




        [HttpGet]
        [Route("health")]
        public  IActionResult healthCheck()
        {
            return Ok(new {health = "healthy"});
        }


        private Response BadAccountCreation()
        {
            return new Response().Status(HttpStatusCode.BadRequest).Message(ResponseMessages.ACCOUNT_CREATION_ERROR);
        }



    }
}
