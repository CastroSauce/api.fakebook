using api.fakebook.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Services.UserService
{
    public interface IUserService
    {
        public Task<ApplicationUser> FindUserByNameAsync(string name);
        public Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password);
        public Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        public Task<IList<string>> GetUserRoles(ApplicationUser user); 
        public Task<ApplicationUser> FindUserById(string id); 


    }
}
