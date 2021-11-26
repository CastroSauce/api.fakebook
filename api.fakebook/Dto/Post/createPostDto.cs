using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Dto.Post
{
    public class createPostDto
    {

        [Required]
        [MaxLength(400)]
        public string text { get; set; }
    }
}
