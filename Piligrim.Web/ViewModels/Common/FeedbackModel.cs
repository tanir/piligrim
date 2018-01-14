using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Piligrim.Web.ViewModels.Common
{
    public class FeedbackModel : IValidatableObject
    {
        [Required(ErrorMessage = "Поле обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        public bool IsCompany { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        [EmailAddress(ErrorMessage = "Укажите корректный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        public string PhoneNumber { get; set; }

        [RegularExpression("^(\\d{10}|\\d{12})$", ErrorMessage = "Введите корректный инн")]
        public string Inn { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsCompany && string.IsNullOrEmpty(Inn))
            {
                yield return new ValidationResult("Необходимо задать ИНН", new[] { nameof(Inn) });
            }
        }
    }
}
