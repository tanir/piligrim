using Microsoft.AspNetCore.Mvc;
using ImageSharp;
using ImageSharp.Processing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Piligrim.Web.Controllers
{
    public class ImageController : Controller
    {
        private readonly IHostingEnvironment env;

        public ImageController(IHostingEnvironment env)
        {
            this.env = env;
        }

        [Route("/image/{width}/{height}/{mode=pad}")]
        [ResponseCache(Duration = 60 * 60 * 24 * 30, Location = ResponseCacheLocation.Any)]

        public async Task<IActionResult> Index(string url, int width, int height, string mode)
        {
            using (var stream = await GetFileStream(url))
            {
                using (var image = Image.Load(stream))
                {
                    var resized = image.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Crop,
                        Size = new Size { Height = height, Width = width }
                    });

                    var outputStream = new MemoryStream();
                    resized.Save(outputStream);

                    outputStream.Seek(0, SeekOrigin.Begin);

                    return this.File(outputStream, "image/jpeg");
                }
            }
        }

        [NonAction]
        private async Task<Stream> GetFileStream(string url)
        {
            if (url.StartsWith("http"))
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(url);

                return await response.Content.ReadAsStreamAsync();
            }
            else
            {
                var path = Path.Combine(this.env.WebRootPath, url.Trim('/').Replace("/", "\\"));

                return new FileStream(path, FileMode.Open);
            }
        }
    }
}