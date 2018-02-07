using Microsoft.EntityFrameworkCore;
using SecurityTokenService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityTokenService.Models
{
    public class TokenDBContext : DbContext
    {
        public DbSet<Token> Token { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=dbsrv-marolt.database.windows.net;Initial Catalog=ProductCatalogeDb;Persist Security Info=True;User ID=db-admin;Password=IEG=shit");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Token>().ToTable("tokens");
            modelBuilder.Entity<Token>().Property(a => a.serviceId).IsRequired();
        }
    }
}
