using Microsoft.AspNetCore.Mvc;

namespace Piligrim.Web.Components
{
    public class CartViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
