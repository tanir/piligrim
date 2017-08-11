using Microsoft.AspNetCore.Mvc;
using ImageSharp;
using ImageSharp.Processing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Extensions.Logging;

namespace Piligrim.Web.Controllers
{
    public class ImageController : Controller
    {
        private static readonly object sync = new object();
        private readonly ILogger logger;
        private readonly IHostingEnvironment env;

        public ImageController(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            this.env = env;
            this.logger = loggerFactory.
                CreateLogger<ImageController>();
        }

        [Route("/image/{width}/{height}/{mode=pad}")]
        [ResponseCache(Duration = 60 * 60 * 24 * 30, Location = ResponseCacheLocation.Any)]

        public IActionResult Index(string url, int width, int height, string mode)
        {
            lock (sync)
            {
                if (string.IsNullOrEmpty(url))
                {
                    return this.BadRequest();
                }

                try
                {
                    using (var stream = GetStream(url).Result)
                    {
                        using (var image = Image.Load(stream))
                        {
                            stream.Dispose();

                            var outputStream = new MemoryStream();

                            using (var resized = image.Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Crop,
                                Size = new SixLabors.Primitives.Size { Width = width, Height = height }
                            }))
                            {
                                this.Response.RegisterForDispose(outputStream);
                                resized.SaveAsJpeg(outputStream);
                            }

                            outputStream.Seek(0, SeekOrigin.Begin);

                            return this.File(outputStream, "image/jpeg");
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(0, ex, $"Произошла ошибка при обработке изображения [{url}]");
                    return this.StatusCode(500);
                }
            }
        }

        [NonAction]
        private async Task<MemoryStream> GetStream(string url)
        {
            if (url.StartsWith("http"))
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);

                return (MemoryStream)await response.Content.ReadAsStreamAsync();
            }
            else
            {
                var path = Path.Combine(this.env.WebRootPath, url.Trim('/').Replace("/", "\\"));

                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous))
                {
                    var result = new MemoryStream();

                    await fileStream.CopyToAsync(result);

                    result.Seek(0, SeekOrigin.Begin);

                    return result;
                }
            }
        }
    }
}