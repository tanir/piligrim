using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Piligrim.Web.Models.Menu;

namespace Piligrim.Web.Components
{
    public class MenuViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            var model = new MenuViewModel();
            model.Categories = new[]
            {
                new MenuCategory
                {
                    Title = "Title1",
                    MenuItems = new[]
                    {
                        new MenuItem {Title = "Menu1", Url = "http://1"},
                        new MenuItem {Title = "Menu2", Url = "http://1"},
                    }
                },
                new MenuCategory
                {
                    Title = "Same"
                },
                new MenuCategory
                {
                    Title = "Title2",
                    MenuItems = new[]
                    {
                        new MenuItem {Title = "Menu3", Url = "http://1"},
                        new MenuItem {Title = "Menu4", Url = "http://1"},
                    }
                }
            };

            return this.View(model);
        }
    }
}
