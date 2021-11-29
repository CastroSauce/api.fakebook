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

        public async Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
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

        public async Task<bool> FollowUser(string followingUserId, string targetUserId)
        {
            if (followingUserId.Equals(targetUserId)) return false;

            var targetUser = await FindUserById(targetUserId);

            if (targetUser == null) return false;

            var sourceUser = await FindUserById(followingUserId);

            var newFollow = new Follow() {follower = sourceUser, followTarget = targetUser};

            await _dbContext.Follows.AddAsync(newFollow);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SendDirectMessage(ClaimsPrincipal user, DirectMessageDto message)
        {

            var to = await FindUserById(message.targetUserId);

            if (to == null) return false;

            var from = await FindUserById(IUserService.GetUserIdFromToken(user));

            var newMessage = new DirectMessage()
            {
                text = message.text,
                from = from,
                to = to,
                sent = DateTime.Now
            };

            await _dbContext.DirectMessages.AddAsync(newMessage);

            await  _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<DirectMessageResponseDto>> GetDirectMessages(ClaimsPrincipal user, string targetUserId)
        {
            var userId = IUserService.GetUserIdFromToken(user);


            return await _dbContext.DirectMessages.AsNoTracking()
                .Where(message => (message.from.Id == targetUserId && message.to.Id == userId) || (message.from.Id == userId && message.to.Id == targetUserId))
                .OrderBy(message => message.sent)
                .Select(message => new DirectMessageResponseDto()
                {
                    text = message.text,
                    sent = message.sent
                }).ToListAsync();
        }

    }
}
