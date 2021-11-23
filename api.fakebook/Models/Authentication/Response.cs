using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Models.Authentication
{
    public class Response
    {
        public string status { get; set; }
        public string message { get; set; }

        public string[] additional { get; set; } = null;

    }

    public static class ResponseCodes
    {
        public const string OK = "SUCCESS";

        public const string ERROR = "ERROR";

    }


    public static class ResponseMessages
    {
        public const string ACCOUNT_CREATION_OK = "Account has been created";
        public const string ACCOUNT_CREATION_ERROR = "Unable to create account";

    }
}
