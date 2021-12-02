using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace api.fakebook.Dto.User
{
    public class DirectMessageResponseDto
    {


        public string username { get; set; }

        public string text { get; set; }

        public DateTime postDate { get; set; }
    }
}
