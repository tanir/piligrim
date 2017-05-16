using System.Collections.Generic;

namespace Piligrim.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Thumbnail { get; set; }

        public string Category { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public virtual ICollection<Size> Sizes { get; set; }

        public virtual ICollection<Color> Colors { get; set; }
    }
}
