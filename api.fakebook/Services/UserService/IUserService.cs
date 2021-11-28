using api.fakebook.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.fakebook.Dto.User;

namespace api.fakebook.Services.UserService
{
    public interface IUserService
    {
        public Task<ApplicationUser> FindUserByNameAsync(string name);
        public Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password);
        public Task<IdentityResult> CreateUserAsync(RegisterModel register);
        public Task<IList<string>> GetUserRoles(ApplicationUser user); 
        public Task<ApplicationUser> FindUserById(string id);
        public Task<bool> FollowUser(ClaimsPrincipal followingUser, string targetUserId);

        public static string GetUserIdFromToken(ClaimsPrincipal User)
        {
           return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }


        public Task<bool> SendDirectMessage(ClaimsPrincipal user, DirectMessageDto message);
        public Task<List<DirectMessageResponseDto>> GetDirectMessages(ClaimsPrincipal user, string targetUserId);
    }
}
