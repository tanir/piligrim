using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Piligrim.Core;
using Piligrim.Web.ViewModels.Product;

namespace Piligrim.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
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
    }

}