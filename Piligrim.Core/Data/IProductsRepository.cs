using System.Collections.Generic;
using System.Threading.Tasks;
using Piligrim.Core.Models;

namespace Piligrim.Core.Data
{
    public interface IProductsRepository
    {
        Task<List<Product>> Find(ProductFilter filter);

        Task<Product> Get(int id);

        Task<IDictionary<int, Product>> Get(int[] id);

        Task Create(Product product);

        Task Update(Product product);
    }
}
