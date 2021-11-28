using api.fakebook.Dto.Post;
using api.fakebook.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Models.PostModels
{
    public class MultiplePostResponse : Response
    {
        public List<ResponsePostDto> posts { get; set; }

        public int available { get; set; }
        public int next { get; set; }
    }
}
