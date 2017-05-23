using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Piligrim.Core;
using Piligrim.Core.Categories;
using Piligrim.Web.ViewModels.Menu;

namespace Piligrim.Web.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ICategoriesProvider categoriesProvider;

        public MenuViewComponent(ICategoriesProvider categoriesProvider)
        {
            this.categoriesProvider = categoriesProvider;
        }

        public IViewComponentResult Invoke()
        {
            var model = new MenuViewModel
            {
                MenuItems = this.categoriesProvider.GetAll().Select(x => this.Build(x))
            };
            
            return this.View(model);
        }

        private MenuItem Build(Category category, Category parent = null)
        {
            return new MenuItem
            {
                Title = category.Title,
                Name = category.Name,
                Url = this.Url.Action("List", "Product", new { category = category.Name, parent = parent?.Name }),
                Child = category.Child.Any()
                    ? category.Child.Select(x => Build(x, category))
                    : Enumerable.Empty<MenuItem>()
            };
        }
    }
}
