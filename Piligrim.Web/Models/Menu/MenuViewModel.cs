using System.Collections.Generic;
using System.Linq;

namespace Piligrim.Web.Models.Menu
{
    public class MenuViewModel
    {
        public IEnumerable<MenuCategory> Categories { get; set; }
    }

    public class MenuCategory
    {
        public string Title { get; set; }

        public bool HasChild
        {
            get { return this.MenuItems != null && this.MenuItems.Any(); }
        }

        public IEnumerable<MenuItem> MenuItems { get; set; }
    }

    public class MenuItem
    {
        public string Url { get; set; }

        public string Title { get; set; }
    }
}
