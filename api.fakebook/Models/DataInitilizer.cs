using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.fakebook.Models
{
    public class DataInitilizer
    {

        public static void seed(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();
        }

    }
}
