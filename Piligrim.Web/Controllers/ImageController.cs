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
            Stream stream;

            if (url.StartsWith("http"))
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(url);

                stream = await response.Content.ReadAsStreamAsync();
            }
            else
            {
                var path = Path.Combine(this.env.ContentRootPath, "wwwroot\\", url.Trim('/').Replace("/", "\\"));
                stream = new FileStream(path, FileMode.Open);
            }

            using (var image = Image.Load(stream))
            {
                var resized = image.Resize(new ResizeOptions
                {
                    Mode = mode == "pad" ? ResizeMode.Pad : ResizeMode.BoxPad,
                    Size = new Size { Height = height, Width = width }
                });

                var outputStream = new MemoryStream();
                resized.Save(outputStream);

                outputStream.Seek(0, SeekOrigin.Begin);

                return this.File(outputStream, "image/jpeg");
            }
        }
    }
}