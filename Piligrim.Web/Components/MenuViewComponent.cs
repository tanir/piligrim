using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Piligrim.Core;
using Piligrim.Web.ViewModels.Menu;

namespace Piligrim.Web.Components
{
    public class MenuViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            var model = new MenuViewModel
            {
                MenuItems = AvailableCategories.Categories.Select(x => this.Build(x))
            };


            return this.View(model);
        }

        private MenuItem Build(Category category, Category parent = null)
        {
            return new MenuItem
            {
                Title = category.Title,
                Url = this.Url.Action("List", "Products", new { category = category.Name, parent = parent?.Name }),
                Child = category.Child.Any()
                    ? category.Child.Select(x => Build(x, category))
                    : Enumerable.Empty<MenuItem>()
            };
        }
    }
}
