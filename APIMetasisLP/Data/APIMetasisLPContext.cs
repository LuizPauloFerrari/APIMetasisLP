using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIMetasisLP.Entities;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace APIMetasisLP.Data
{
    public class APIMetasisLPContext : DbContext
    {
        public APIMetasisLPContext (DbContextOptions<APIMetasisLPContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(message => Debug.WriteLine(message), LogLevel.Information);

        public DbSet<APIMetasisLP.Entities.Produto> Produto { get; set; }
    }
}
