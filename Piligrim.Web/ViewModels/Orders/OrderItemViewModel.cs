namespace Piligrim.Web.ViewModels.Orders
{
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }

        public string Title { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}