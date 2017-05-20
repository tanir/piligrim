using System.ComponentModel.DataAnnotations;
using Piligrim.Core.Models;

namespace Piligrim.Web.ViewModels.Cart
{
    public class AddOrderViewModel
    {
        [Required(ErrorMessage = "Укажите email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Укажите валидный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression("+?\\d{10,15}")]
        public string Telephone { get; set; }

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
