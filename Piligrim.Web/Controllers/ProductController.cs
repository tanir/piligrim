using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Piligrim.Core.Categories;
using Piligrim.Core.Data;
using Piligrim.Core.Models;
using Piligrim.Web.ViewModels.Product;

namespace Piligrim.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsRepository productsRepository;
        private readonly IHostingEnvironment env;
        private readonly ICategoriesProvider categoriesProvider;

        public ProductController(
            IProductsRepository productsRepository,
            IHostingEnvironment env,
            ICategoriesProvider categoriesProvider)
        {
            this.productsRepository = productsRepository;
            this.env = env;
            this.categoriesProvider = categoriesProvider;
        }

        [Route("products/{parent}/{category}/", Order = 1)]
        [Route("products/{category}/", Order = 2)]
        [Route("[controller]/[action]", Order = 3)]
        public async Task<IActionResult> List(string category, string search)
        {
            var currentCategory = this.categoriesProvider.Get(category);

            this.ViewData["Title"] = search ?? (currentCategory?.Title ?? "Список товаров");

            var filter = new ProductFilter { SearchKeyword = search, Category = search == null ? category : null };

            var products = await this.productsRepository.Find(filter);

            var model = products.Select(x => new ProductsListViewModel(x.Id, x.Thumbnail, x.Price, x.Title, x.Unit));

            return this.View(model);
        }

        [Route("product/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await this.productsRepository.Get(id);

            var model = new ProductDetailsViewModel
            {
                Id = product.Id,
                ColorSizes = product.Colors.Select(x => new KeyValuePair<string, IEnumerable<string>>
                    (
                        x.Value,
                        x.Sizes.Select(y => y.Value).ToList()
                    ))
                    .ToDictionary(x => x.Key, x => x.Value),
                Title = product.Title,
                Description = product.Description.Replace("\r\n", "<br/>"),
                Price = product.Price,
                Photos = product.Photos.Select(x => x.Uri).ToList(),
                Thumbnail = product.Thumbnail,
                Deleted = product.Deleted,
                Unit = product.Unit
            };

            return this.View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            CreateOrEditProductViewModel model;

            if (id.HasValue)
            {
                var product = await this.productsRepository.Get(id.Value).ConfigureAwait(false);

                model = new CreateOrEditProductViewModel
                {
                    Id = product.Id,
                    Title = product.Title,
                    SizeColors = new SizeColor { Values = product.Colors },
                    Price = product.Price,
                    Thumbnail = product.Thumbnail,
                    Photos = product.Photos?.Select(x => x.Uri).ToList() ?? new List<string>(),
                    Category = product.Category,
                    Description = product.Description,
                    Unit = product.Unit
                };
            }
            else
            {
                model = new CreateOrEditProductViewModel();
            }

            this.ViewBag.Categories = new SelectList(this.categoriesProvider.Leafs(), "Name", "Title", model.Category);

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrEdit(CreateOrEditProductViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            Product product;

            if (model.Id.HasValue)
            {
                product = await this.productsRepository.Get(model.Id.Value).ConfigureAwait(false);
            }
            else
            {
                product = new Product();
            }

            product.Title = model.Title;
            product.Price = model.Price;

            product.Colors = model.SizeColors.Values.ToList();

            product.Category = model.Category;
            product.Description = model.Description;
            product.Unit = model.Unit;

            await (model.Id.HasValue
                ? this.productsRepository.Update(product)
                : this.productsRepository.Create(product));

            return this.RedirectToAction(model.Id.HasValue ? "Details" : "CreateOrEdit", new { id = product.Id });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Upload(int productId)
        {
            return this.View(new UploadViewModel { ProductId = productId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upload(UploadViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var product = await this.productsRepository.Get(model.ProductId).ConfigureAwait(false);

            if (model.Photos != null)
            {
                var photoUris = await this.SaveFiles(model.Photos.ToArray()).ConfigureAwait(false);

                foreach (var photo in photoUris.Select(x => new Photo { Uri = x }))
                {
                    product.Photos.Add(photo);
                }
            }

            if (model.Thumbnail != null)
            {
                var thumbnailUri = await this.SaveFiles(model.Thumbnail).ConfigureAwait(false);

                product.Thumbnail = thumbnailUri.Single();
            }

            await this.productsRepository.Update(product);

            return this.RedirectToAction("CreateOrEdit", new { id = model.ProductId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePhoto(int productId, string photoUri)
        {
            var product = await this.productsRepository.Get(productId).ConfigureAwait(false);

            var deletePhoto = product.Photos.FirstOrDefault(x => x.Uri == photoUri);

            if (deletePhoto == null)
            {
                this.ModelState.AddModelError("photoUri", "Продукт не содержит такой фотографии");
                return this.View("Edit", new { id = productId });
            }

            product.Photos.Remove(deletePhoto);

            await this.productsRepository.Update(product).ConfigureAwait(false);

            return this.RedirectToAction("CreateOrEdit", new { id = productId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await this.productsRepository.Get(id).ConfigureAwait(false);

            product.Deleted = true;

            await this.productsRepository.Update(product);

            return this.RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Restore(int id)
        {
            var product = await this.productsRepository.Get(id).ConfigureAwait(false);

            product.Deleted = false;

            await this.productsRepository.Update(product);

            return this.RedirectToAction("Details", new { id });
        }

        [NonAction]
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