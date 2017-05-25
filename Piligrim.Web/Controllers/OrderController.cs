using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Piligrim.Core.Data;
using Piligrim.Core.Mail;
using Piligrim.Core.Models;
using Piligrim.Web.Configuration;
using Piligrim.Web.ViewModels.Cart;

namespace Piligrim.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IProductsRepository productsRepository;
        private readonly IEmailService emailService;
        private readonly IOptions<AppSettings> appSettings;
        private readonly IHostingEnvironment env;

        public OrderController(
            IOrdersRepository ordersRepository,
            IProductsRepository productsRepository,
            IEmailService emailService,
            IOptions<AppSettings> appSettings,
            IHostingEnvironment env)
        {
            this.ordersRepository = ordersRepository;
            this.productsRepository = productsRepository;
            this.emailService = emailService;
            this.appSettings = appSettings;
            this.env = env;
        }

        public IActionResult Index()
        {
            var model = new AddOrderViewModel();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(AddOrderViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Index", model);
            }

            var products = await this.productsRepository.Get(model.OrderItems.Select(x => x.Id).ToArray())
                .ConfigureAwait(false);

            var order = new Order
            {
                CustomerName = model.CustomerName,
                Address = model.Address,
                Comment = model.Comment,
                Delivery = model.Delivery,
                Email = model.Email,
                Payment = model.Payment,
                PhoneNumber = model.PhoneNumber,
                OrderItems = model.OrderItems
                    .Select(x => new OrderItem
                    {
                        Color = x.Color,
                        Size = x.Size,
                        Count = x.Count,
                        Price = x.Price,
                        Product = products[x.Id]
                    })
                    .ToList()
            };

            var added = await this.ordersRepository.Add(order).ConfigureAwait(false);


            var templatePath = "Emails\\NewOrder.cshtml";

            await this.emailService.Send(
                order,
                this.appSettings.Value.EmailForOrders,
                this.appSettings.Value.PhoneNumber,
                templatePath,
                env.ContentRootPath);

            await this.emailService.Send(
                this.appSettings.Value.EmailForOrders,
                "Поступил новый заказ",
                $"Поступил новый заказ {added.Id}");

            return this.RedirectToAction("Success", new SuccessViewModel { OrderId = added.Id });
        }

        public IActionResult Success(SuccessViewModel model)
        {
            return this.View(model);
        }
    }
}