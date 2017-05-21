using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Piligrim.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Piligrim.Core.Data;
using Piligrim.Web.ViewModels.Orders;

namespace Piligrim.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private const int PageSize = 20;

        private readonly IOrdersRepository ordersRepository;

        public OrdersController(IOrdersRepository ordersRepository)
        {
            this.ordersRepository = ordersRepository;
        }

        public async Task<IActionResult> List(int page = 1)
        {
            var filter = new OrderFilter
            {
                Omit = (page - 1) * PageSize,
                Select = PageSize,
                Status = null
            };

            var orders = await this.ordersRepository.Find(filter).ConfigureAwait(false);

            var order = orders.Select(x => new OrderListItemViewModel
            {
                Id = x.Id,
                Status = x.Status,
                Email = x.Email,
                Timestamp = x.Timestamp
            }).ToList();

            var model = new OrderListViewModel { Orders = order };

            ViewBag.Page = page;

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await this.ordersRepository.Get(id).ConfigureAwait(false);
            var model = new OrderViewModel
            {
                Id = order.Id,
                Timestamp = order.Timestamp,
                Status = order.Status,
                Email = order.Email,
                Comment = order.Comment,
                Delivery = order.Delivery,
                PhoneNumber = order.PhoneNumber,
                DeliveryComment = order.DeliveryComment,
                Payment = order.Payment,
                OrderItems = order.OrderItems.Select(x => new OrderItemViewModel
                {
                    Title = x.Product.Title,
                    Color = x.Color,
                    Size = x.Size,
                    Count = x.Count,
                    Price = x.Price,
                    ProductId = x.Product.Id
                }).ToList()
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Details(UpdateOrderViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var order = await this.ordersRepository.Get(model.Id).ConfigureAwait(false);
            order.Status = model.Status;
            order.DeliveryComment = model.DeliveryComment;

            await this.ordersRepository.Update(order).ConfigureAwait(false);

            return this.RedirectToAction("List");
        }
    }
}