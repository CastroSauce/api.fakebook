using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Dto.User
{
    public class DirectMessageDto
    {

        [Required]
        public string targetUsername { get; set; }

        [Required]
        public string text { get; set; }
    }
}
