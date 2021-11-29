using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.fakebook.Dto.User
{
    public class FollowDto
    {
        [Required]
        public string targetUserId{ get; set; }      
    }
}
