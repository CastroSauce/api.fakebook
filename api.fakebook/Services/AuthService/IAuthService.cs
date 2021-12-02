using api.fakebook.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Services.AuthService
{
    public interface IAuthService
    {
        public  JwtSecurityToken GenerateJwtToken(ApplicationUser user, IList<string> roles);

        public Task<LoginResponse> Authenticate(LoginModel login);

        public Task<RegisterResponse> Register(RegisterModel register);
    }
}
