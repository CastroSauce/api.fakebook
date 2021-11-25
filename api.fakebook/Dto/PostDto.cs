using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Dto
{
    public class PostDto
    {
        public Guid publicId { get; set; }

        public DateTime postDate { get; set; }

        public string text { get; set; }

        public string postedBy { get; set; }
    }
}
