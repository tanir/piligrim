using Microsoft.AspNetCore.Mvc;

namespace Piligrim.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}