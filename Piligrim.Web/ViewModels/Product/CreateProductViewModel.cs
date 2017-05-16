using System.ComponentModel.DataAnnotations;

namespace Piligrim.Web.ViewModels.Product
{
    public class CreateProductViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Sizes { get; set; }

        [Required]
        public string Colors { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
