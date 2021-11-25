﻿using api.fakebook.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.extensions
{
    public static class ResponseExtensions
    {

        public static Response Error(this Response responseObj)
        {
            responseObj.status = ResponseCodes.ERROR;
            return responseObj;
        }
        public static Response Ok(this Response responseObj)
        {
            responseObj.status = ResponseCodes.OK;
            return responseObj;
        }

        public static Response Status(this Response responseObj, ResponseCodes Status)
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

        public static Response IdentityErrors(this Response responseObj, IEnumerable<IdentityError> errors)
        {
            var errorList = errors.Select(error => error.Description).ToList();
            responseObj.Additional(new {IdentityErrors = errorList, });
            return responseObj;
        }



    }
}
