using System.Threading.Tasks;
using Piligrim.Core.Models;

namespace Piligrim.Core
{
    public interface IOrdersRepository
    {
        Task<Order> Add(Order order);
    }
}
