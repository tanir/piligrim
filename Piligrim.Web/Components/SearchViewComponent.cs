using Microsoft.AspNetCore.Mvc;

namespace Piligrim.Web.Components
{
    public class SearchViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var search = this.Request.Query["search"].ToString();

            return this.View((object)search);
        }
    }
}
