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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using api.fakebook.Models.PostModels;

namespace api.fakebook.Services.PostService
{
    public class PostService : IPostService
    {

        public int limit { get; } = 10;
        private ApplicationDbContext _dbContext { get; set; }
        private IUserService _userService { get; set; }


        public PostService(ApplicationDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public async Task<List<ResponsePostDto>> GetPostsByUsernameAsync(string username, int offset)
        {
            //TODO move to repository
            var query = _dbContext.Posts.AsNoTracking().Where(Post => Post.postedBy.UserName == username).Skip(offset).Take(limit);

            return await  query.Select(Post => new ResponsePostDto()
                {
                    Id = Post.publicId,
                    postDate = Post.postDate,
                    userId = Post.postedBy.Id,
                    username = Post.postedBy.UserName,
                    text = Post.text
                }).ToListAsync();
        }

        public async Task<ResponsePostDto> CreatePostAsync(createPostDto createPost, ClaimsPrincipal userToken)
        {
            var userId = IUserService.GetUserIdFromToken(userToken);

            var user = await _userService.FindUserById(userId);

            var postEntity = createPost.toPostBase().toPost(user);

            await _dbContext.Posts.AddAsync(postEntity);

            await _dbContext.SaveChangesAsync();

            await CheckIfNeedToCreateMentionPost(postEntity);

            return postEntity.toDto();
        }

        private async Task CheckIfNeedToCreateMentionPost(Post post)
        {
            var mentionedUser = await CheckForMention(post.text);

            if (mentionedUser == null) return;

            await CreateMentionPost(post, mentionedUser);
        }

       
        private async Task<ApplicationUser> CheckForMention(string postText)
        {
            var mention = postText.Split(" ").FirstOrDefault(word => word.StartsWith("@"));

            if (mention == null) return null;

            return await _userService.FindByUsernameAsync(mention.Substring(1)); // remove @
        }

        private async Task CreateMentionPost(Post post, ApplicationUser user)
        {
            var mentionPost = new Mention()
            {
                mentionPost = post,
                mentionedUser = user
            };

            await _dbContext.Mentions.AddAsync(mentionPost);

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

            var followedPosts = await GetFollowedPosts(userId);

            var mentionPosts = await GetMentionPosts(userId);

            var posts = followedPosts.Concat(mentionPosts).ToList();

            posts.OrderBy(post => post.postDate);

            return posts;
        }

        private async Task<List<ResponsePostDto>> GetFollowedPosts(string userId)
        {
            //TODO move to repository
            var follows = _dbContext.Follows.Where(follow => follow.follower.Id == userId)
                .Select(follow => follow.followTarget.Id).AsQueryable();

            return await _dbContext.Posts.AsNoTracking()
                .Where(post => follows.Contains(post.postedBy.Id))
                .Take(limit)
                .Select(post => new ResponsePostDto()
                {
                    Id = post.publicId,
                    postDate = post.postDate,
                    userId = post.postedBy.Id,
                    username = post.postedBy.UserName,
                    text = post.text
                }).ToListAsync();
        }

        private async Task<List<ResponsePostDto>> GetMentionPosts(string userId)
        {
            //TODO move to repository
           

            return await _dbContext.Mentions.AsNoTracking()
                .Where(mention => mention.mentionedUser.Id == userId)
                .Take(limit)
                .Select(mention => new ResponsePostDto()
                {
                    Id = mention.mentionPost.publicId,
                    postDate = mention.mentionPost.postDate,
                    userId = mention.mentionPost.postedBy.Id,
                    username = mention.mentionPost.postedBy.UserName,
                    text = mention.mentionPost.text
                }).ToListAsync();
        }

        public async Task<ResponsePostDto> GetPostById(string postId)
        {
            //TODO move to repository
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

        public async Task<int> GetPostsAvailableByUsernameAsync(string username)
        {
            return await _dbContext.Posts.Where(Post => Post.postedBy.Id == username).CountAsync();
        }


    }
}
