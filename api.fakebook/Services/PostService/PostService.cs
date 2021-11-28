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

        public int limit { get; } = 10;
        private ApplicationDbContext _dbContext { get; set; }
        private IUserService _userManager { get; set; }


        public PostService(ApplicationDbContext dbContext, IUserService userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<ResponsePostDto>> GetPostsByUserIdAsync(string id, int offset)
        {
            var query = _dbContext.Posts.AsNoTracking().Where(Post => Post.postedBy.Id == id).Skip(offset).Take(limit);

            return await  query.Select(Post => new ResponsePostDto()
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

            var user = await _userManager.FindUserById(userId);

            var postEntity = postDto.toPost(user);

            await _dbContext.AddAsync(postEntity);

            await _dbContext.SaveChangesAsync();

        }

        public async Task<int> GetWallPostsAvailable(ClaimsPrincipal userToken)
        {
            var userId = IUserService.GetUserIdFromToken(userToken);

            var follows = _dbContext.Follows.Where(follow => follow.follower.Id == userId)
                .Select(follow => follow.followTarget.Id).AsQueryable();

            return await _dbContext.Posts.Where(post => follows.Contains(post.postedBy.Id)).CountAsync();
        }

        public async Task<List<ResponsePostDto>> GetWall(ClaimsPrincipal userToken, int offset)
        {
            var userId = IUserService.GetUserIdFromToken(userToken);


            var follows =  _dbContext.Follows.Where(follow => follow.follower.Id == userId)
                .Select(follow => follow.followTarget.Id).AsQueryable();

            var posts = await _dbContext.Posts.AsNoTracking()
                .Where(post => follows.Contains(post.postedBy.Id))
                .Take(limit)
                .OrderBy(post => post.postDate)
                .Select(post => new ResponsePostDto()
                {
                    Id = post.publicId,
                    postDate = post.postDate,
                    userId = post.postedBy.Id,
                    username = post.postedBy.UserName,
                    text = post.text
                }).ToListAsync();

            return posts;
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

        public async Task<int> GetPostsAvailableByUserIdAsync(string userId)
        {
            return await _dbContext.Posts.Where(Post => Post.postedBy.Id == userId).CountAsync();
        }
    }
}
