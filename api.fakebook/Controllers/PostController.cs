using api.fakebook.Dto;
using api.fakebook.Dto.Post;
using api.fakebook.extensions;
using api.fakebook.Models;
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

        [HttpGet("PostsByUserId")]
        public async Task<IActionResult> GetPosts(string userId, int offset = 0)
        {
            var posts = await _postService.GetPostsByUserIdAsync(userId);

            if (posts.Count == 0) return NoContent();

            return  Ok(posts);
        }

        [HttpGet("PostById")]
        public async Task<IActionResult> GetPostById(string postId)
        {
            var post = await _postService.GetPostById(postId);

            if (post == null) return NoContent();

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody]createPostDto post)
        {
            var basicPost = post.toPostBase();

             await _postService.CreatePostAsync(basicPost, User);

            return CreatedAtAction(nameof(GetPostById), new { postId = basicPost.Id }, basicPost);
        }
    }
}
