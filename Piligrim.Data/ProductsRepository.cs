using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Piligrim.Core;
using Piligrim.Core.Data;
using Piligrim.Core.Models;

namespace Piligrim.Data
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly StoreDbContext dbContext;

        public ProductsRepository(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<List<Product>> Find(ProductFilter filter)
        {
            var categories = new List<string>();

            if (filter.Category != null)
            {
                categories.Add(filter.Category);
            }

            var products = this.dbContext.Products.Where(x => !x.Deleted);

            if (!string.IsNullOrEmpty(filter.SearchKeyword))
            {
                products = products.Where(
                    x => x.Title.Contains(filter.SearchKeyword) || x.Description.Contains(filter.SearchKeyword));
            }

            if (categories.Any())
            {
                products = products.Where(x => categories.Contains(x.Category));
            }

            return products.ToListAsync();
        }

        public Task<Product> Get(int id)
        {
            return this.dbContext.Products.Include(x => x.Colors)
                .Include(x => x.Sizes)
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IDictionary<int, Product>> Get(int[] ids)
        {
            var products = await this.dbContext.Products.Include(x => x.Colors)
                .Include(x => x.Sizes)
                .Include(x => x.Photos)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync()
                .ConfigureAwait(false);

            return products.ToDictionary(p => p.Id, p => p);
        }

        public async Task Create(Product product)
        {
            await this.dbContext.Products.AddAsync(product);
            await this.dbContext.SaveChangesAsync();
        }

        public Task Update(Product product)
        {
            this.dbContext.Products.Update(product);
            return this.dbContext.SaveChangesAsync();
        }
    }
}
