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


        [Required]
        public string text { get; set; }
        [Required]
        public DateTime sent { get; set; }
    }
}
