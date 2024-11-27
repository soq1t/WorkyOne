using System.ComponentModel.DataAnnotations;

namespace WorkyOne.Contracts.Attributes.Validation
{
    public class ShiftTimeAttribute : ValidationAttribute
    {
        private string _comparedPropertyName;
        private string? _errorMessage;

        public ShiftTimeAttribute(string comparedPropertyName, string? errorMessage = null)
        {
            _comparedPropertyName = comparedPropertyName;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            var comparedProperty = validationContext.ObjectType.GetProperty(_comparedPropertyName);

            var comparedValue = (TimeOnly?)
                comparedProperty.GetValue(validationContext.ObjectInstance);

            if (value != null && comparedValue == null)
            {
                return new ValidationResult(
                    _errorMessage ?? $"Property {_comparedPropertyName} is required"
                );
            }

            return ValidationResult.Success;
        }
    }
}
