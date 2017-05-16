using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Piligrim.Core;
using Piligrim.Core.Models;
using Piligrim.Web.ViewModels.Product;

namespace Piligrim.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IHostingEnvironment env;

        public ProductController(IProductRepository productRepository, IHostingEnvironment env)
        {
            this.productRepository = productRepository;
            this.env = env;
        }

        [Route("products/{category}/{parent?}/", Order = 1)]
        [Route("[controller]/[action]", Order = 2)]
        public async Task<IActionResult> List(string category, string search, string parent)
        {
            var currentCategory = AvailableCategories.Categories.FirstOrDefault(x => x.Name == category);

            ViewData["Title"] = search ?? (currentCategory?.Title ?? "Список товаров");

            var filter = new ProductFilter { SearchKeyword = search, Category = category };

            var products = await this.productRepository.Find(filter);

            var model = products.Select(x => new ProductsListViewModel(x.Id, x.Thumbnail, x.Price, x.Title));

            return this.View(model);
        }

        [Route("products/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await this.productRepository.Get(id);

            var model = new ProductDetailsViewModel
            {
                Id = product.Id,
                Colors = product.Colors.Select(x => x.Value).ToList(),
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Photos = product.Photos.Select(x => x.Uri).ToList(),
                Sizes = product.Sizes.Select(x => x.Value).ToList(),
                Thumbnail = product.Thumbnail
            };

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View(new CreateProductViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            return this.RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await this.productRepository.Get(id).ConfigureAwait(false);

            var model = new EditProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Colors = string.Join(";", product.Colors.Select(x => x.Value)),
                Sizes = string.Join(";", product.Sizes.Select(x => x.Value)),
                Price = product.Price,
                Thumbnail = product.Thumbnail,
                Photos = product.Photos.Select(x => x.Uri).ToList()
            };

            return this.View(model);
        }

        public IActionResult Upload(int productId)
        {
            return this.View(new UploadViewModel { ProductId = productId });
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var product = await this.productRepository.Get(model.ProductId).ConfigureAwait(false);

            if (model.Photos != null)
            {
                var photoUris = await this.SaveFiles(model.Photos.ToArray()).ConfigureAwait(false);

                product.Photos.AddRange(photoUris.Select(x => new Photo { Uri = x }));
            }

            if (model.Thumbnail != null)
            {
                var thumbnailUri = await this.SaveFiles(model.Thumbnail).ConfigureAwait(false);

                product.Thumbnail = thumbnailUri.Single();
            }

            await this.productRepository.Update(product);

            return this.RedirectToAction("Edit", new { id = model.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var product = await this.productRepository.Get(model.Id).ConfigureAwait(false);

            product.Title = model.Title;
            product.Price = model.Price;
            product.Colors = model.Colors.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new Color { Value = x })
                .ToList();

            product.Sizes = model.Sizes.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new Size { Value = x })
                .ToList();


            return this.RedirectToAction("List");
        }

        public async Task<IActionResult> DeletePhoto(int productId, string photoUri)
        {
            var product = await this.productRepository.Get(productId).ConfigureAwait(false);

            var deletePhoto = product.Photos.FirstOrDefault(x => x.Uri == photoUri);

            if (deletePhoto == null)
            {
                this.ModelState.AddModelError("photoUri", "Продукт не содержит такой фотографии");
                return this.View("Edit", new { id = productId });
            }

            product.Photos.Remove(deletePhoto);

            await this.productRepository.Update(product).ConfigureAwait(false);

            return this.RedirectToAction("Edit", new { id = productId });
        }

        private async Task<IEnumerable<string>> SaveFiles(params IFormFile[] uploadedFiles)
        {
            var imageRootFolder = this.env.WebRootPath;

            var fileRelativeUris = new List<string>();

            foreach (var photo in uploadedFiles)
            {
                if (photo.Length > 0)
                {
                    using (var sha1 = SHA1.Create())
                    {
                        var hash = BitConverter.ToString(sha1.ComputeHash(photo.OpenReadStream())).Replace("-", string.Empty);

                        var fileName = hash + Path.GetExtension(photo.FileName);

                        var imageRelative = Path.Combine("upload/images/", fileName);

                        var filePath = Path.Combine(imageRootFolder, imageRelative);

                        fileRelativeUris.Add(imageRelative);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream).ConfigureAwait(false);
                        }
                    }
                }
            }

            return fileRelativeUris.Select(x => Path.Combine("/", x));
        }
    }
}