using Piligrim.Core.Models;

namespace Piligrim.Core
{
    public class OrderFilter
    {
        public int Select { get; set; }

        public int Omit { get; set; }

        public OrderStatus? Status { get; set; }
    }
}
