using api.fakebook.Dto;
using api.fakebook.Models;
using api.fakebook.Services.PostService;
using api.fakebook.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private  IPostService _postService {get; init;}
        public PostController(IPostService userService)
        {
            _postService = userService;
        }

        [HttpGet("userId")]
        public async Task<IActionResult> GetPosts(String userId)
        {
            var posts = await _postService.GetPostsByUserId(userId);

            if (posts.Count == 0) return NoContent();

            return  Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody]PostDto post)
        {
            if(!ModelState.IsValid) return BadRequest();

            return Ok();

        }






    }
}
