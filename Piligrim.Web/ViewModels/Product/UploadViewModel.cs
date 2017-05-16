using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Piligrim.Web.ViewModels.Product
{
    public class UploadViewModel
    {
        public int ProductId { get; set; }

        public IFormFile Thumbnail { get; set; }

        public List<IFormFile> Photos { get; set; }
    }
}
