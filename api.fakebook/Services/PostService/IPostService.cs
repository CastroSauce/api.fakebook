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
        public Task<List<ResponsePostDto>> GetPostsByUserIdAsync(string userId);
        public Task<ResponsePostDto> GetPostById(string postId);
        public Task CreatePostAsync(BasePostDto post, ClaimsPrincipal userToken);

 
    }
}
