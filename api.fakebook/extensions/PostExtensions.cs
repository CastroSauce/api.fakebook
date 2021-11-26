using api.fakebook.Dto;
using api.fakebook.Dto.Post;
using api.fakebook.Models;
using api.fakebook.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.extensions
{
    public static class PostExtensions
    {
        public static ResponsePostDto toDto(this Post post)
        {
            return new ResponsePostDto()
            {
                postDate = post.postDate,
                text = post.text,
                Id = post.publicId,
                username = post.postedBy.UserName,
                userId = post.postedBy.Id
            };
        }

        public static BasePostDto toPostBase(this createPostDto post)
        {
            return new BasePostDto()
            {
                postDate = DateTime.Now,
                text = post.text,
                Id = Guid.NewGuid()
            };
        }

        public static Post toPost(this BasePostDto post, ApplicationUser user)
        {
            return new Post()
            {
                postDate = post.postDate,
                text = post.text,
                publicId = post.Id,
                postedBy = user

            };
        }

    }
}
