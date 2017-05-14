using System.Collections.Generic;

namespace Piligrim.Core.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Comment { get; set; }

        public string Delivery { get; set; }

        public string Payment { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}