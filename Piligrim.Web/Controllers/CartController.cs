using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Piligrim.Core;
using Piligrim.Core.Models;
using Piligrim.Web.ViewModels.Cart;

namespace Piligrim.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IOrdersRepository ordersRepository;

        public CartController(IOrdersRepository ordersRepository)
        {
            this.ordersRepository = ordersRepository;
        }
        
        public IActionResult Index()
        {
            var model = new AddOrderViewModel();
            return this.View(model);
        }

        [HttpPost]
        public IActionResult CreateOrder(AddOrderViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Index", model);
            }

            var order = new Order
            {
                Address = model.Address,
                Comment = model.Comment,
                Delivery = model.Delivery,
                Email = model.Email,
                Payment = model.Payment,
                PhoneNumber = model.Telephone,
                OrderItems = model.OrderItems
                    .Select(x => new OrderItem
                    {
                        Color = x.Color,
                        Size = x.Size,
                        Count = x.Count,
                        Price = x.Price,
                        ProductId = x.Id
                    })
                    .ToList()
            };

            var added = this.ordersRepository.Add(order);

            return this.RedirectToAction("Success", new SuccessViewModel { OrderId = added.Id });
        }

        public IActionResult Success(SuccessViewModel model)
        {
            return this.View(model);
        }
    }
}