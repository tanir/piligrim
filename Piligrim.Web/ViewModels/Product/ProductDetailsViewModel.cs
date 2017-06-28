using System.Collections.Generic;

namespace Piligrim.Web.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Photos { get; set; }

        public string Thumbnail { get; set; }

        public bool Deleted { get; set; }

        public IDictionary<string, IEnumerable<string>> ColorSizes { get; set; }

        public string Unit { get; set; }
    }
}
