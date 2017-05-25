using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Piligrim.Core.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string Comment { get; set; }

        public DeliveryMethod Delivery { get; set; }

        public PaymentMethod Payment { get; set; }

        public string DeliveryComment { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Timestamp { get; set; }

        public OrderStatus Status { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}