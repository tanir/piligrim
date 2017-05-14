using Microsoft.EntityFrameworkCore;
using Piligrim.Core.Models;

namespace Piligrim.Data
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}