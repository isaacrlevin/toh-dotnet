using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tohdotnet;

namespace tohdotnet.domain.Models
{
    public class tohdotnetContext : DbContext
    {
        public tohdotnetContext(DbContextOptions<tohdotnetContext> options)
            : base(options)
        {
        }

        public DbSet<Hero> Heroes { get; set; }
    }
}
