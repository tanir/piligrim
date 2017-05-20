using System.Collections.Generic;

namespace Piligrim.Web.ViewModels.Orders
{
    public class OrderListViewModel
    {
        public IEnumerable<OrderListItemViewModel> Orders { get; set; }
    }
}
