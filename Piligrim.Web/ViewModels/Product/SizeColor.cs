using System.Collections.Generic;

namespace Piligrim.Web.ViewModels.Product
{
    public class SizeColor
    {
        public string Size { get; set; }

        public IEnumerable<string> Colors { get; set; }
    }
}