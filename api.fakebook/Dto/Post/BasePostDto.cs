using System;

namespace api.fakebook.Dto.Post
{
    public class BasePostDto
    {
        public Guid Id { get; set; }

        public DateTime postDate { get; set; }

        public string text { get; set; }
    }
}