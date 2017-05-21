using System.ComponentModel.DataAnnotations;
using Piligrim.Core.Models;

namespace Piligrim.Web.ViewModels.Cart
{
    public class AddOrderViewModel
    {
        [Required(ErrorMessage = "Укажите email")]
        [EmailAddress(ErrorMessage = "Укажите валидный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Comment { get; set; }

        [Required(ErrorMessage = "Выберите способ доставки")]
        public DeliveryMethod Delivery { get; set; }

        [Required(ErrorMessage = "Выберите способ оплаты")]
        public PaymentMethod Payment { get; set; }

        [Required(ErrorMessage = "Нет товаров")]
        public OrderItemViewModel[] OrderItems { get; set; }
    }
}
