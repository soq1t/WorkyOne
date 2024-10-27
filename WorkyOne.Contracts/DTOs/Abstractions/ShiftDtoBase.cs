using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.Attributes.Validation;

namespace WorkyOne.Contracts.DTOs.Abstractions
{
    /// <summary>
    /// База DTO для сущностей смен
    /// </summary>
    public abstract class ShiftDtoBase : DtoBase
    {
        /// <summary>
        /// Название смены
        /// </summary>
        [Required]
        [Length(1, 30)]
        public string Name { get; set; }

        /// <summary>
        /// Цветовой код смены
        /// </summary>
        [Required]
        [Length(
            4,
            7,
            ErrorMessage = "Цветовой код должен содержать 4 либо 7 символов (#FFF или #FFFFFF)"
        )]
        [RegularExpression(
            @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
            ErrorMessage = "Цветовой код должен представлять формат HEX (#FFFFFF или #FFF)"
        )]
        public string? ColorCode { get; set; } = "#FFFFFF";

        /// <summary>
        /// Время начала смены
        /// </summary>
        [ShiftTime(nameof(Ending))]
        public TimeOnly? Beginning { get; set; }

        /// <summary>
        /// Время окончания смены
        /// </summary>
        [ShiftTime(nameof(Beginning))]
        public TimeOnly? Ending { get; set; }

        /// <summary>
        /// Длительность смены в формате TimeSpan
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}
