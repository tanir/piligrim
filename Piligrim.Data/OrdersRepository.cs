using System.Threading.Tasks;
using Piligrim.Core;
using Piligrim.Core.Models;

namespace Piligrim.Data
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ProductsDbContext context;

        public OrdersRepository(ProductsDbContext context)
        {
            this.context = context;
        }

        public async Task<Order> Add(Order order)
        {
            var added = await this.context.Orders.AddAsync(order);

            return added.Entity;
        }
    }
}
