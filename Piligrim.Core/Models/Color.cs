using System.Collections.Generic;

namespace Piligrim.Core.Models
{
    public class Color
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public virtual ICollection<Size> Sizes { get; set; }
    }
}