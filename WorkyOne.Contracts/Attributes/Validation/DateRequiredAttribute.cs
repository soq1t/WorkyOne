using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Contracts.Attributes.Validation
{
    /// <summary>
    /// Аттрибут для валидации минимально допустимого значения даты
    /// </summary>
    public class DateRequiredAttribute : ValidationAttribute
    {
        protected readonly DateOnly _minimumDate;

        public DateRequiredAttribute(DateTime dateTime)
        {
            _minimumDate = DateOnly.FromDateTime(dateTime);
        }

        public DateRequiredAttribute(int year, int month, int days)
        {
            _minimumDate = new DateOnly(year, month, days);
        }

        public DateRequiredAttribute()
        {
            var date = DateOnly.FromDateTime(DateTime.Now);
            _minimumDate = date.AddYears(-100);
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            DateOnly? date = (DateOnly?)value;

            if (!date.HasValue)
            {
                return new ValidationResult(
                    $"The {validationContext.DisplayName} field is required."
                );
            }
            else if (date.Value < _minimumDate)
            {
                return new ValidationResult(
                    $"The {validationContext.DisplayName} field's value must be more than {_minimumDate.ToShortDateString()}."
                );
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
