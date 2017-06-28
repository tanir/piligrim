using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Piligrim.Core;
using Piligrim.Core.Data;
using Piligrim.Core.Models;

namespace Piligrim.Data
{
    public class OrdersRepository : IOrdersRepository,IDisposable
    {
        private readonly StoreDbContext context;

        public OrdersRepository(StoreDbContext context)
        {
            this.context = context;
        }

        public async Task<Order> Add(Order order)
        {
            var added = await this.context.Orders.AddAsync(order);

            await this.context.SaveChangesAsync();

            return added.Entity;
        }

        public Task<Order> Get(int id)
        {
            return this.context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).SingleAsync(x => x.Id == id);
        }

        public Task<List<Order>> Find(OrderFilter filter)
        {
            return this.context.Orders.Where(order => !filter.Status.HasValue || order.Status == filter.Status)
                .OrderByDescending(x => x.Timestamp)
                .Skip(filter.Omit)
                .Take(filter.Select)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task Update(Order order)
        {
            this.context.Orders.Update(order);
            return this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.context.SaveChanges();

            this.context?.Dispose();
        }
    }
}
