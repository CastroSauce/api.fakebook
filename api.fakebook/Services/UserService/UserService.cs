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

        public async Task<ApplicationUser> FindByUsernameAsync(string username)
        {


            return await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == username);

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
                Email = "somethingsomething@something.com",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            //return await _userManager.CreateAsync(user, register.Password);
            return await _userManager.CreateAsync(user, "PlaceHolder@12345");
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        
        }

        public async Task<ApplicationUser> FindUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> FollowUser(string followingUsername, string targerUsername)
        {
            if (followingUsername.Equals(targerUsername)) return false;

            var targetUser = await FindByUsernameAsync(targerUsername);

            if (targetUser == null) return false;

            var sourceUser = await FindByUsernameAsync(followingUsername);

            var newFollow = new Follow() {follower = sourceUser, followTarget = targetUser};

            await _dbContext.Follows.AddAsync(newFollow);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SendDirectMessage(ClaimsPrincipal user, DirectMessageDto message)
        {

            var to = await FindByUsernameAsync(message.targetUsername);

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

        public async Task<List<DirectMessageResponseDto>> GetDirectMessages(ClaimsPrincipal user, string targetUsername)
        {
            var username = IUserService.GetUsername(user);


            return await _dbContext.DirectMessages.AsNoTracking()
                .Where(message => (message.from.UserName == targetUsername && message.to.UserName == username) || (message.from.UserName == username && message.to.UserName == targetUsername))
                .OrderBy(message => message.sent)
                .Select(message => new DirectMessageResponseDto()
                {
                    username = message.from.UserName,
                    text = message.text,
                    postDate = message.sent
                }).ToListAsync();
        }

    }
}
