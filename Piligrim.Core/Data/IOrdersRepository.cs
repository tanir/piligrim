using System.Collections.Generic;
using System.Threading.Tasks;
using Piligrim.Core.Models;

namespace Piligrim.Core.Data
{
    public interface IOrdersRepository
    {
        Task<Order> Add(Order order);

        Task<Order> Get(int id);

        Task<List<Order>> Find(OrderFilter filter);

        Task Update(Order order);
    }
}
