using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public IEnumerable<Tuple<string, string>> AvailableDeliveries { get; } = new[]
        {
            Tuple.Create("pickup", "Самовывоз"),
            Tuple.Create("courier", "Доставка курьерской службой")
        };

        [Required(ErrorMessage = "Выберите способ доставки")]
        public string Delivery { get; set; }

        public IEnumerable<Tuple<string, string>> AvailablePayments { get; } = new[]
        {
            Tuple.Create("forDetails", "Оплата по реквизитам"),
            Tuple.Create("uponReceipt", "При получении")
        };

        [Required(ErrorMessage = "Выберите способ оплаты")]
        public string Payment { get; set; }

        [Required(ErrorMessage = "Нет товаров")]
        public OrderItemViewModel[] OrderItems { get; set; }
    }
}
