using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.fakebook.Models.Authentication;

namespace api.fakebook.Models.PostModels
{
    public class Mention
    {
        public int Id { get; set; }

        public Post mentionPost { get; set; }

        public ApplicationUser mentionedUser { get; set; }

    }
}
