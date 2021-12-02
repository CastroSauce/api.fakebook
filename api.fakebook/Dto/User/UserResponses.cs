using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.fakebook.Models.Authentication;

namespace api.fakebook.Models.UserModels
{
    public class UserResponses : Response
    {


    }

    class UserResponseMessages
    {
        public const string FOLLOW_SUCCESS = "user followed";
        public const string FOLLOW_FAILED = "Follow failed";

    }
}
