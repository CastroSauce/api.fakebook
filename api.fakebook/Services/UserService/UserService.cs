using api.fakebook.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Services.UserService
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager { get; init; }

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> FindUserByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);

        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        
        }

        public async Task<ApplicationUser> FindUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
    }
}
