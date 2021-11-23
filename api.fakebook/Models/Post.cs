using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Models
{
    public class Post
    {
        public int Id { get; set; }

        public DateTime postDate { get; set; }

        public string text { get; set; }

        public User postedBy { get; set; }
    }
}
