using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.Enums.Attributes;

namespace WorkyOne.Contracts.Attributes.Validation
{
    public sealed class DateCompareAttribute : ValidationAttribute
    {
        private readonly DateCompareMode _mode;

        private readonly string _comparePropertyName;

        public DateCompareAttribute(DateCompareMode mode, string comparePropertyName)
        {
            _mode = mode;
            _comparePropertyName = comparePropertyName;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            var compareProperty = validationContext.ObjectType.GetProperty(_comparePropertyName);

            if (compareProperty == null)
            {
                return new ValidationResult($"Неизвестное свойство: {_comparePropertyName}");
            }

            var compareValue = (DateOnly?)
                compareProperty.GetValue(validationContext.ObjectInstance);

            var currentValue = (DateOnly?)value;

            if (compareValue == null)
            {
                return new ValidationResult($"{_comparePropertyName} must be not null");
            }

            if (currentValue == null)
            {
                return new ValidationResult($"{validationContext.MemberName} must be not null");
            }

            switch (_mode)
            {
                case DateCompareMode.More:
                    if (currentValue > compareValue)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(
                            $"Ошибка: значение должно быть больше чем {_comparePropertyName}"
                        );
                    }
                case DateCompareMode.Less:
                    if (currentValue < compareValue)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(
                            $"Ошибка: значение должно быть меньше чем {_comparePropertyName}"
                        );
                    }
                case DateCompareMode.MoreOrEquial:
                    if (currentValue >= compareValue)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(
                            $"Ошибка: значение должно быть больше или равно {_comparePropertyName}"
                        );
                    }
                case DateCompareMode.LessOrEquial:
                    if (currentValue <= compareValue)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(
                            $"Ошибка: значение должно быть меньше или равно {_comparePropertyName}"
                        );
                    }
                case DateCompareMode.Equial:
                    if (currentValue == compareValue)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(
                            $"Ошибка: значение должно быть равно {_comparePropertyName}"
                        );
                    }
                case DateCompareMode.NotEquial:
                    if (currentValue != compareValue)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(
                            $"Ошибка: значение не должно быть равно {_comparePropertyName}"
                        );
                    }

                default:
                    return new ValidationResult($"Неизвестная ошибка");
            }
        }
    }
}
