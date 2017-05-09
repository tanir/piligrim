using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Piligrim.Core;
using Piligrim.Core.Models;

namespace Piligrim.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsDbContext dbContext;

        public ProductRepository(ProductsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Product>> Find(ProductFilter filter)
        {
            var categories = new HashSet<string>();

            if (filter.Category != null)
            {
                categories.Add(filter.Category);
            }

            if (filter.ParentCategory != null)
            {
                var parentCategory =
                    AvailableCategories.Categories.FirstOrDefault(x => x.Name == filter.ParentCategory);

                if (parentCategory != null)
                {
                    categories.UnionWith(parentCategory.Child.Select(x => x.Name));
                }
            }

            var products = await this.dbContext.Products.Where(x => (
                                                          filter.SearchKeyword == null
                                                          || x.Title.Contains(filter.SearchKeyword)
                                                          || x.Description.Contains(filter.SearchKeyword)) &&
                                                      (!categories.Any() || categories.Contains(x.Category)))
                                            .ToListAsync();

            return products;
        }

        public Task<Product> Get(int id)
        {
            return this.dbContext.Products.Include(x => x.Colors)
                .Include(x => x.Sizes)
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
