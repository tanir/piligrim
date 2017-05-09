namespace Piligrim.Web.ViewModels.Product
{
    public class ProductsListViewModel
    {
        public ProductsListViewModel(int id, string thumbnail, decimal price, string title)
        {
            this.Id = id;
            this.Thumbnail = thumbnail;
            this.Price = price;
            this.Title = title;
        }

        public int Id { get; set; }

        public string Thumbnail { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }
    }
}
