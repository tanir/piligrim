using System.ComponentModel.DataAnnotations;
using Piligrim.Core.Models;

namespace Piligrim.Web.ViewModels.Orders
{
    public class UpdateOrderViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public string DeliveryComment { get; set; }
    }
}
