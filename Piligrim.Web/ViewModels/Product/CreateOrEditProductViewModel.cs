using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Piligrim.Web.ViewModels.Product
{
    public class CreateOrEditProductViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string SizeColors { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        public List<string> Photos { get; set; }

        public string Thumbnail { get; set; }

        public string Unit { get; set; }
    }
}
