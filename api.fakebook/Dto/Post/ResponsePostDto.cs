using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Dto.Post
{
    public class ResponsePostDto : BasePostDto
    {
        public string username { get; set; }
        public string userId { get; set; }
    }
}
