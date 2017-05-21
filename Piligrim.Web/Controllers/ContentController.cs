using Microsoft.AspNetCore.Mvc;

namespace Piligrim.Web.Controllers
{
    public class ContentController : Controller
    {
        [Route("content/{page}")]
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

            return this.View(view);
        }
    }
}