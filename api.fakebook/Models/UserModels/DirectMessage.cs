using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.fakebook.Models.Authentication;

namespace api.fakebook.Models.UserModels
{
    public class DirectMessage
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string text { get; set; }
        [Required]
        public ApplicationUser from { get; set; }
        [Required]
        public ApplicationUser to { get; set; }

        [Required]
        public DateTime sent { get; set; }

    }
}
