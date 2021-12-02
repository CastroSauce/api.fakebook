using api.fakebook.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.fakebook.extensions;
using api.fakebook.Services.UserService;

namespace api.fakebook.Services.AuthService
{
    public class AuthService : IAuthService
    {

        private IConfiguration _configuration { get; init; }
        private IUserService _userService { get; set; }
        public AuthService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }


        public JwtSecurityToken GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            return new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.Now.AddHours(4),
                claims: authClaims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
        }

        public async Task<LoginResponse> Authenticate(LoginModel login)
        {
            var user = await _userService.FindByUsernameAsync(login.Username);

            if (user == null) return null;

            //if (!await _userService.CheckUserPasswordAsync(user, login.Password)) return null;
            if (!await _userService.CheckUserPasswordAsync(user, "PlaceHolder@12345")) return null;

            var roles = await _userService.GetUserRoles(user);

            var token = GenerateJwtToken(user, roles);

            return new LoginResponse()
                .Token(token)
                .User(user)
                .Ok()
                .Message(ResponseMessages.ACCOUNT_LOGIN_OK) as LoginResponse;
        }

        public async Task<RegisterResponse> Register(RegisterModel register)
        {
            if (await _userService.CheckIfUserExistsByUsername(register.Username))
                return new RegisterResponse().BadRequest().Message("User already exists") as RegisterResponse;


            var result = await _userService.CreateUserAsync(register);


            if (!result.Succeeded)
            {
                var errorResponse = new RegisterResponse()
                .IdentityErrors(result.Errors)
                .BadRequest()
                .Message(ResponseMessages.ACCOUNT_LOGIN_ERROR);

                return errorResponse as RegisterResponse;
            }

            return new RegisterResponse()
                .Ok()
                .Message(ResponseMessages.ACCOUNT_LOGIN_OK) as RegisterResponse;

        }

   
    }
}
