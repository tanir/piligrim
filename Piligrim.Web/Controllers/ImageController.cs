using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Piligrim.Web.Controllers
{
    public class ImageController : Controller
    {
        private static readonly object sync = new object();

        private static readonly Size maxSize = new Size(1920, 1080);

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
            if (string.IsNullOrEmpty(url))
            {
                return this.BadRequest();
            }

            lock (sync)
            {

                try
                {
                    using (var stream = GetStream(url).Result)
                    {
                        using (var image = Image.Load(stream))
                        {
                            stream.Dispose();

                            var outputStream = new MemoryStream();

                            var restrictedPath = this.GetRestrictedPath(url);

                            if (restrictedPath != null &&
                                (image.Width > maxSize.Width || image.Height > maxSize.Height))
                            {
                                using (var clonedImage = image.Clone())
                                {
                                    clonedImage.Mutate(x => x.Resize(new ResizeOptions
                                    {
                                        Mode = ResizeMode.Max,
                                        Size = maxSize
                                    }));

                                    clonedImage.Save(restrictedPath);
                                }
                            }

                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Crop,
                                Size = new Size { Width = width, Height = height }
                            }));

                            this.Response.RegisterForDispose(outputStream);
                            image.SaveAsJpeg(outputStream, new JpegEncoder { Quality = 80 });


                            outputStream.Seek(0, SeekOrigin.Begin);

                            return this.File(outputStream, "image/png");
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, $"Произошла ошибка при обработке изображения [{url} - {ex}]");
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

                var restrictedSizePath = this.GetRestrictedPath(url);

                if (restrictedSizePath != null && System.IO.File.Exists(restrictedSizePath))
                {
                    path = restrictedSizePath;
                }

                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous))
                {
                    var result = new MemoryStream();

                    await fileStream.CopyToAsync(result);

                    result.Seek(0, SeekOrigin.Begin);

                    return result;
                }
            }
        }

        [NonAction]
        private string GetRestrictedPath(string url)
        {
            var sourceFileName = Path.Combine(this.env.WebRootPath, url.Trim('/').Replace("/", "\\"));
            var withoutExtension = Path.GetFileNameWithoutExtension(sourceFileName);
            var extension = Path.GetExtension(sourceFileName);

            return url.StartsWith("http")
                ? null
                : Path.Combine(Path.GetDirectoryName(sourceFileName), $"{withoutExtension}_{maxSize.Width}x{maxSize.Height}{extension}");
        }
    }
}