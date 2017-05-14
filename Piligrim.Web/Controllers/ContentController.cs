using Microsoft.AspNetCore.Mvc;

namespace Piligrim.Web.Controllers
{
    [Route("content")]
    public class ContentController : Controller
    {
        [Route("{page}")]
        public IActionResult Index(string page)
        {
            string view = null;

            switch (page)
            {
                case "kak-sdelat-zakaz":
                    view = "order";
                    break;
                case "dostavka":
                    view = "delivery";
                    break;
                case "kak-oplatit-zakaz":
                    view = "payment";
                    break;
                case "o-nas":
                    view = "aboutus";
                    break;
            }

            if (string.IsNullOrEmpty(view))
            {
                return this.NotFound();
            }

            return View(view);
        }
    }
}