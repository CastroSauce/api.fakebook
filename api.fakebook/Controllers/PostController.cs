using api.fakebook.Dto;
using api.fakebook.Dto.Post;
using api.fakebook.extensions;
using api.fakebook.Models.PostModels;
using api.fakebook.Services.PostService;
using api.fakebook.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.fakebook.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private  IPostService _postService {get; init;}
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("PostsByUsername")]
        public async Task<IActionResult> GetPosts(string username, int offset = 0)
        {


            var posts = await _postService.GetPostsByUsernameAsync(username, offset);

            if (posts.Count == 0) return NoContent();

            var available = await _postService.GetPostsAvailableByUsernameAsync(username);

            var nextAvailable = available < offset + _postService.limit ? available : offset + _postService.limit;

            var response = new MultiplePostResponse()
                .AddPosts(posts, available, nextAvailable)
                .Ok();

            return Ok(response);
        }

        [HttpGet("PostById")]
        public async Task<IActionResult> GetPostById(string postId)
        {
            var post = await _postService.GetPostById(postId);

            return post != null ? Ok(post) : NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody]createPostDto post)
        {

             var result = await _postService.CreatePostAsync(post, User);

            return CreatedAtAction(nameof(GetPostById), new { postId = result.Id }, result);
        }

        [HttpGet("Wall")]
        public async Task<IActionResult> GetWall(int offset = 0)
        {
            var posts = await _postService.GetWall(User, offset);

            if (posts.Count == 0) return NoContent();

            var available = await _postService.GetWallPostsAvailable(User);

            var nextAvailable = available < offset + _postService.limit ? available : offset + _postService.limit;

            var response = new MultiplePostResponse()
                .AddPosts(posts, available, nextAvailable)
                .Ok();

            return Ok(response);
        }
    }
}
