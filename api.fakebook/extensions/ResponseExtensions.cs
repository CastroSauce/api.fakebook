using api.fakebook.Models.Authentication;
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
            responseObj.token = new JwtSecurityTokenHandler().WriteToken(token);
            responseObj.expire = token.ValidTo;
            return responseObj;
        }



    }
}
