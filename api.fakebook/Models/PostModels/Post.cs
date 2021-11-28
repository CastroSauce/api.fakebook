using api.fakebook.Models.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public Guid publicId { get; set; }

        [Required]
        public DateTime postDate { get; set; }

        [Required]
        public string text { get; set; }

        [Required]
        public ApplicationUser postedBy { get; set; }
    }
}
