using api.fakebook.Dto;
using api.fakebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.extensions
{
    public static class PostExtensions
    {
        public static PostDto toDto(this Post post)
        {
            return new PostDto()
            {
                postDate = post.postDate,
                text = post.text,
                postedBy = post.postedBy.UserName,
                publicId = post.publicId
            };
        }
    }
}
