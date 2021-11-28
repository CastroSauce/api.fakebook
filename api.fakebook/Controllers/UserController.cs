using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.fakebook.extensions;
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
        public async Task<IActionResult> FollowUser(string targetUserId)
        {

            var succsess = await _userService.FollowUser(User,targetUserId);

            if (!succsess)
            {
                var response = new UserResponses().BadRequest().Message(UserResponseMessages.FOLLOW_FAILED);
                return BadRequest(response);
            }

            return Ok(new UserResponses().Ok());
        }




    }
}
