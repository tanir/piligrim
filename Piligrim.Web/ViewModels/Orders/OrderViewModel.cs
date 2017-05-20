using System;
using System.Collections.Generic;
using System.Linq;
using Piligrim.Core.Models;

namespace Piligrim.Web.ViewModels.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Comment { get; set; }

        public DeliveryMethod Delivery { get; set; }

        public PaymentMethod Payment { get; set; }

        public DateTime Timestamp { get; set; }

        public OrderStatus Status { get; set; }

        public string DeliveryComment { get; set; }

        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }

        public decimal Cost
        {
            get { return this.OrderItems.Sum(x => x.Price * x.Count); }
        }
    }
}