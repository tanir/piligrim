using System.ComponentModel.DataAnnotations;

namespace Piligrim.Core.Models
{
    public enum OrderStatus
    {
        [Display(Name = "Новый")] New = 0,
        [Display(Name = "Отправлен")] Sent = 1,
        [Display(Name = "Завершен")] Completed = 2
    }
}