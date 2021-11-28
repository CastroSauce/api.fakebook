using api.fakebook.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using api.fakebook.Dto.User;
using api.fakebook.Models;
using api.fakebook.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace api.fakebook.Services.UserService
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager { get; init; }

        private ApplicationDbContext _dbContext { get; init; }

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser> FindUserByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);

        }

        public async Task<IdentityResult> CreateUserAsync(RegisterModel register)
        {

            ApplicationUser user = new()
            {
                UserName = register.Username,
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            return await _userManager.CreateAsync(user, register.Password);
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        
        }

        public async Task<ApplicationUser> FindUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> FollowUser(ClaimsPrincipal followingUser, string targetUserId)
        {
            var targetUser = await FindUserById(targetUserId);

            if (targetUser == null) return false;

            var sourceUser = await FindUserById(IUserService.GetUserIdFromToken(followingUser));

            var newFollow = new Follow() {follower = sourceUser, followTarget = targetUser};

            await _dbContext.AddAsync(newFollow);

            return true;
        }

        public async Task<bool> SendDirectMessage(ClaimsPrincipal user, DirectMessageDto message)
        {
            var from = await FindUserById(IUserService.GetUserIdFromToken(user));

            var to = await FindUserById(message.targetUserId);

            if (from == null || to == null) return false;


            var newMessage = new DirectMessage()
            {
                text = message.text,
                from = from,
                to = to,
                sent = DateTime.Now
            };

            await _dbContext.AddAsync(newMessage);

            await  _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<DirectMessageResponseDto>> GetDirectMessages(ClaimsPrincipal user, string targetUserId)
        {
            var thisUser = await FindUserById(IUserService.GetUserIdFromToken(user));

            var targetUser = await FindUserById(targetUserId);

            return await _dbContext.DirectMessages.AsNoTracking()
                .Where(message => message.to == thisUser && message.from == targetUser)
                .Select(message => new DirectMessageResponseDto()
                {
                    text = message.text,
                    sent = message.sent
                }).ToListAsync();
        }
    }
}
