using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Piligrim.Web.ViewModels.Product
{
    public class EditProductViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Sizes { get; set; }

        [Required]
        public string Colors { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IEnumerable<string> Photos { get; set; }

        public string Thumbnail { get; set; }
    }
}
