using System;
using System.Collections.Generic;
using Piligrim.Core.Models;

namespace Piligrim.Web.ViewModels.Emails
{
    public class NewOrderViewModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string CustomerAddress { get; set; }

        public DateTime Created { get; set; }

        public decimal Total { get; set; }

        public IEnumerable<NewOrderItemViewModel> Items { get; set; }
    }
}
