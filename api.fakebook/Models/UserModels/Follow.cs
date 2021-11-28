using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using api.fakebook.Models.Authentication;

namespace api.fakebook.Models.UserModels
{
    public class Follow
    {
        public int Id { get; set; }

        public ApplicationUser follower { get; set; }

        public ApplicationUser followTarget { get; set; }
    }
}
