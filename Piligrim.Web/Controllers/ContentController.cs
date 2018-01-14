using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Piligrim.Core.Mail;
using Piligrim.Web.Configuration;
using Piligrim.Web.ViewModels.Common;

namespace Piligrim.Web.Controllers
{
    public class ContentController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IOptions<AppSettings> _appSettings;

        public ContentController(IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _emailService = emailService;
            _appSettings = appSettings;
        }

        [Route("content/optovikam")]
        public IActionResult WholeSalers()
        {
            return this.View();
        }

        [Route("content/optovikam")]
        [HttpPost]
        public async Task<IActionResult> SendFeedback(FeedbackModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View("Wholesalers");
            }

            var body =
                $@"Имя: {model.Name}, Email: {model.Email}, Номер телефона: {model.PhoneNumber}, 
                    ИНН: {model.Inn}, {(model.IsCompany ? "Юридическое лицо" : "Физическое лицо")}";

            await this._emailService.Send(this._appSettings.Value.EmailForOrders, "Заявка от оптовика", body);

            return this.RedirectToAction("Index", "Home");
        }

        [Route("content/dostavka")]
        public IActionResult Delivery()
        {
            return this.View();
        }

        [Route("content/kak-oplatit-zakaz")]
        public IActionResult Payment()
        {
            return this.View();
        }
    }
}