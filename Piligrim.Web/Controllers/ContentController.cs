using Microsoft.AspNetCore.Mvc;

namespace Piligrim.Web.Controllers
{
    public class ContentController : Controller
    {
        [Route("content/optovikam")]
        public IActionResult WholeSalers()
        {
            return this.View();
        }

        [Route("content/dostavka")]
        public IActionResult Delivery()
        {
            return this.View();
        }

        [Route("content/kak-oplatit-zakaz")]
        public IActionResult Payment()
        {
            return this.View();
        }
    }
}