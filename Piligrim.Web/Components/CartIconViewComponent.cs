using Microsoft.AspNetCore.Mvc;

namespace Piligrim.Web.Components
{
    public class CartIconViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
