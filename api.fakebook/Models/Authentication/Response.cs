using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Models.Authentication
{
    public class Response
    {
        public ResponseCodes status { get; set; }
        public string message { get; set; }

        public List<object> additional { get; set; } = new();

    }

    public enum ResponseCodes
    {
        OK = 200,
        ERROR = 400,
    }


    public static class ResponseMessages
    {
        public const string ACCOUNT_CREATION_OK = "Account has been created";
        public const string ACCOUNT_CREATION_ERROR = "Unable to create account";
        public const string ACCOUNT_LOGIN_ERROR = "Username or password is wrong";
        public const string ACCOUNT_LOGIN_OK = "Login successfull";
    }
}
