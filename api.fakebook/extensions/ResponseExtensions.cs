using api.fakebook.Dto.Post;
using api.fakebook.Models.Authentication;
using api.fakebook.Models.PostModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace api.fakebook.extensions
{
    public static class ResponseExtensions
    {

        public static Response BadRequest(this Response responseObj)
        {
            responseObj.status = HttpStatusCode.BadRequest;
            return responseObj;
        }
        public static Response Ok(this Response responseObj)
        {
            responseObj.status = HttpStatusCode.OK;
            return responseObj;
        }

        public static Response Status(this Response responseObj, HttpStatusCode Status)
        {
            responseObj.status = Status;
            return responseObj;
        }

        public static Response Message(this Response responseObj, string ErrorMessage)
        {
            responseObj.message = ErrorMessage;
            return responseObj;
        }

        public static Response Additional(this Response responseObj, object Additional)
        {
            responseObj.additional.Add(Additional);
            return responseObj;
        }

        public static RegisterResponse IdentityErrors(this RegisterResponse responseObj, IEnumerable<IdentityError> errors)
        {
            var errorList = errors.Select(error => error.Description).ToList();
            responseObj.errors = errorList;
            return responseObj;
        }

        public static LoginResponse Token(this LoginResponse responseObj, JwtSecurityToken token)
        {
            responseObj.Token = new JwtSecurityTokenHandler().WriteToken(token);
            responseObj.Expire = token.ValidTo;
            return responseObj;
        }

        public static LoginResponse User(this LoginResponse responseObj, ApplicationUser user)
        {
            responseObj.Id = user.Id;
            responseObj.Username = user.UserName;
            return responseObj;
        }

        public static RegisterResponse User(this RegisterResponse responseObj, ApplicationUser user)
        {
            responseObj.userId = user.Id;
            responseObj.username = user.UserName;
            return responseObj;
        }

        public static MultiplePostResponse AddPosts(this MultiplePostResponse responseObj, List<ResponsePostDto> posts, int available, int next)
        {
            responseObj.available = available;
            responseObj.next = next;
            responseObj.posts = posts;
            return responseObj;
        }

    }
}
