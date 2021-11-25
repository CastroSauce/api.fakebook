using api.fakebook.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Services.PostService
{
    public interface IPostService
    {
        public Task<List<PostDto>> GetPostsByUserId(string id);

 
    }
}
