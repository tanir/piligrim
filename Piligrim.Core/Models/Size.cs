using System.Collections.Generic;

namespace Piligrim.Core.Models
{
    public class Size
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public virtual ICollection<Color> Colors { get; set; }
    }
}