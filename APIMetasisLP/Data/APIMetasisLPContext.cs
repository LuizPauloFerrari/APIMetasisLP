using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIMetasisLP.Entities;

namespace APIMetasisLP.Data
{
    public class APIMetasisLPContext : DbContext
    {
        public APIMetasisLPContext (DbContextOptions<APIMetasisLPContext> options)
            : base(options)
        {
        }

        public DbSet<APIMetasisLP.Entities.Produto> Produto { get; set; }
    }
}
