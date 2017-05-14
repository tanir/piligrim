namespace Piligrim.Core.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }
    }
}