using System.Collections.Generic;
using System.Threading.Tasks;
using Piligrim.Core.Models;

namespace Piligrim.Core
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> Find(ProductFilter filter);

        Task<Product> Get(int id);
    }
}
