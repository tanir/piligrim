using System;
using Piligrim.Core.Models;

namespace Piligrim.Web.ViewModels.Orders
{
    public class OrderListItemViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public DateTime Timestamp { get; set; }

        public OrderStatus Status { get; set; }
    }
}