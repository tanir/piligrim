using System.Collections.Generic;
using System.Linq;

namespace Piligrim.Core.Categories
{
    public class Category
    {
        public Category(string name, string title)
        {
            this.Name = name;
            this.Title = title;
        }

        public string Name { get; set; }

        public string Title { get; set; }

        public IEnumerable<Category> Child { get; set; } = Enumerable.Empty<Category>();
    }
}