using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Piligrim.Core;
using Piligrim.Web.ViewModels.Product;

namespace Piligrim.Web.Controllers
{
    [Route("/products")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [Route("{category}/{parent?}/")]
        public async Task<IActionResult> List(string category, string search, string parent)
        {
            var filter = new ProductFilter { SearchKeyword = search, Category = category };

            var products = await this.productRepository.Find(filter);

            var model = products.Select(x => new ProductsListViewModel(x.Id, x.Thumbnail, x.Price, x.Title));

            return this.View(model);
        }

        [Route("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await this.productRepository.Get(id);

            var model = new ProductDetailsViewModel
            {
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