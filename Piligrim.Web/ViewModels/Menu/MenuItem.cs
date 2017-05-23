using System.Collections.Generic;
using System.Linq;

namespace Piligrim.Web.ViewModels.Menu
{
    public class MenuItem
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public IEnumerable<MenuItem> Child { get; set; }

        public string Name { get; set; }

        public bool HasChild => this.Child != null && this.Child.Any();
    }
}