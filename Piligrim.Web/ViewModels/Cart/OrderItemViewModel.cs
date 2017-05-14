using System.ComponentModel.DataAnnotations;

namespace Piligrim.Web.ViewModels.Cart
{
    public class OrderItemViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
    }
}