using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Piligrim.Data;

namespace Piligrim.Web.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddProductsDbContext(this IServiceCollection collection, string connectionString)
        {
            collection.AddDbContext<StoreDbContext>(builder =>
                builder.UseSqlServer(connectionString,
                    options => options.MigrationsAssembly("Piligrim.Web")));
        }
    }
}
