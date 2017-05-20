using System.ComponentModel.DataAnnotations;

namespace Piligrim.Core.Models
{
    public enum DeliveryMethod
    {
        [Display(Name = "Самовывоз")]
        Pickup = 1,

        [Display(Name = "Доставка курьерской службой")]
        Courier = 2
    }
}
