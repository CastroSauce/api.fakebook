using api.fakebook.Dto;
using api.fakebook.extensions;
using api.fakebook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Services.PostService
{
    public class PostService : IPostService
    {
        private ApplicationDbContext _dbContext { get; set; }
        public PostService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PostDto>> GetPostsByUserId(string id)
        {
            return await _dbContext.Posts.AsNoTracking()
                .Where(Post => Post.postedBy.Id == id)
                .Select(Post => Post.toDto()).ToListAsync();
        }
    }
}
