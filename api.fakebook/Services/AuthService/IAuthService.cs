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
        public Task<ApplicationUser> FindUserByNameAsync(string name);
        public Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password);
        public Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        public Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user);

    }
}
