using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogeLocal.Models
{
    public class ProductDBContext : DbContext
    {
        public DbSet<Product> products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=dbsrv-marolt.database.windows.net;Initial Catalog=ProductCatalogeDb;Persist Security Info=True;User ID=db-admin;Password=IEG=shit");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Product>().Property(a => a.ProductName).IsRequired();
        }
    }
}
