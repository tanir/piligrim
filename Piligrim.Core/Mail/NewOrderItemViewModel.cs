namespace Piligrim.Web.ViewModels.Emails
{
    public class NewOrderItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}