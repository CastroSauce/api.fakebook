using api.fakebook.Dto;
using api.fakebook.Dto.Post;
using api.fakebook.Models;
using api.fakebook.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.fakebook.Services.PostService
{
    public interface IPostService
    {

        public int limit { get; }

        public Task<List<ResponsePostDto>> GetPostsByUsernameAsync(string username, int offset);
        public Task<int> GetPostsAvailableByUsernameAsync(string username);
        public Task<ResponsePostDto> GetPostById(string postId);
        public Task<ResponsePostDto> CreatePostAsync(createPostDto createPost, ClaimsPrincipal userToken);
        public Task<int> GetWallPostsAvailable(ClaimsPrincipal userToken);
        public Task<List<ResponsePostDto>> GetWall(ClaimsPrincipal userToken, int offset);
    }
}
