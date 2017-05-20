using System.ComponentModel.DataAnnotations;

namespace Piligrim.Core.Models
{
    public enum PaymentMethod
    {
        [Display(Name = "Оплата по реквизитам")]
        ForDetails = 1,
        [Display(Name = "При получении")]
        OnReceipt = 2
    }
}