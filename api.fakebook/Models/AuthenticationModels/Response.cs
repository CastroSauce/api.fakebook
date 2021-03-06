using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace api.fakebook.Models.Authentication
{
    public class Response
    {
        public HttpStatusCode status { get; set; }
        public string message { get; set; }

        public List<object> additional { get; set; } = new();
    }


    public class LoginResponse : Response
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime Expire { get; set; }
    }

    public class RegisterResponse : Response
    {
        public string username  { get; set; }
        public string userId  { get; set; }

        public List<string> errors = new();
    }

    public static class ResponseMessages
    {
        public const string ACCOUNT_CREATION_OK = "Account has been created";
        public const string ACCOUNT_CREATION_ERROR = "Unable to create account";
        public const string ACCOUNT_LOGIN_ERROR = "Username or password is wrong";
        public const string ACCOUNT_LOGIN_OK = "Login successfull";
    }
}
