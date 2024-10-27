using System.ComponentModel.DataAnnotations;

namespace WorkyOne.Contracts.Attributes.Validation
{
    public class ShiftTimeAttribute : ValidationAttribute
    {
        private string _comparedPropertyName;

        public ShiftTimeAttribute(string comparedPropertyName)
        {
            _comparedPropertyName = comparedPropertyName;
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
                return new ValidationResult($"Property {_comparedPropertyName} is required");
            }

            return ValidationResult.Success;
        }
    }
}
