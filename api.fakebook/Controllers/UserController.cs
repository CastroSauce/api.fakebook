using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.fakebook.Dto.User;
using api.fakebook.extensions;
using api.fakebook.Models.PostModels;
using api.fakebook.Models.UserModels;
using api.fakebook.Services.UserService;
using Microsoft.AspNetCore.Authorization;

namespace api.fakebook.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public IUserService _userService{ get; set; }  
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("follow")]
        public async Task<IActionResult> FollowUser([FromBody]FollowDto follow)
        {

            var username = IUserService.GetUsername(User);

            if (username.Equals(follow.targetUsername)) return BadRequest(new UserResponses().BadRequest().Message(UserResponseMessages.FOLLOW_FAILED));

            var success = await _userService.FollowUser(username, follow.targetUsername);

            if (!success)
            {
                var response = new UserResponses().BadRequest().Message(UserResponseMessages.FOLLOW_FAILED);
                return BadRequest(response);
            }

            return Ok(new UserResponses().Ok());
        }

        [HttpPost("directMessage")]
        public async Task<IActionResult> SendDirectMessage([FromBody]DirectMessageDto message)
        {
            var success = await _userService.SendDirectMessage(User, message);

            if (!success) return BadRequest();

            return Ok();
        }

        [HttpGet("directMessage")]
        public async Task<IActionResult> GetDirectMessages(string targetUsername)
        {
            var result = await _userService.GetDirectMessages(User, targetUsername);

            if (result.Count == 0) return NoContent();

            return Ok(result);
        }

    }
}
