using api.fakebook.Dto;
using api.fakebook.Dto.Post;
using api.fakebook.extensions;
using api.fakebook.Models;
using api.fakebook.Models.Authentication;
using api.fakebook.Services.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.fakebook.Services.PostService
{
    public class PostService : IPostService
    {
        private ApplicationDbContext _dbContext { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }

        public PostService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<ResponsePostDto>> GetPostsByUserIdAsync(string id)
        {
            return await _dbContext.Posts.AsNoTracking()
                .Where(Post => Post.postedBy.Id == id)
                .Select(Post => new ResponsePostDto()
                {
                    Id = Post.publicId,
                    postDate = Post.postDate,
                    userId = Post.postedBy.Id,
                    username = Post.postedBy.UserName,
                    text = Post.text
                }).ToListAsync();
        }

        public async Task CreatePostAsync(BasePostDto postDto, ClaimsPrincipal userToken)
        {

            var userId = IUserService.GetUserIdFromToken(userToken);

            var user = await _userManager.FindByIdAsync(userId);

            var PostEntity = postDto.toPost(user);

            await _dbContext.AddAsync(PostEntity);

            await _dbContext.SaveChangesAsync();

            return;
        }

        public async Task<ResponsePostDto> GetPostById(string postId)
        {
            return await _dbContext.Posts.AsNoTracking().Include(post => post.postedBy)
                .Where(Post => Post.publicId.ToString().Equals(postId))
                .Select(Post => new ResponsePostDto() { 
                    Id = Post.publicId,
                    postDate = Post.postDate,
                    userId = Post.postedBy.Id,
                    username = Post.postedBy.UserName,
                    text = Post.text
                }).FirstOrDefaultAsync();
        }
    }
}
