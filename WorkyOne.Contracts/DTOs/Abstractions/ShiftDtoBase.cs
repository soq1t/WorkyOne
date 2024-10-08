using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Contracts.DTOs.Abstractions
{
    /// <summary>
    /// База DTO для сущностей смен
    /// </summary>
    public abstract class ShiftDtoBase : DtoBase
    {
        /// <summary>
        /// ID смены
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название смены
        /// </summary>
        [Required]
        [Length(1, 30)]
        public string Name { get; set; }

        /// <summary>
        /// Цветовой код смены
        /// </summary>
        [Length(
            4,
            7,
            ErrorMessage = "Цветовой код должен содержать 4 либо 7 символов (#FFF или #FFFFFF)"
        )]
        [RegularExpression(
            @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
            ErrorMessage = "Цветовой код должен представлять формат HEX (#FFFFFF или #FFF)"
        )]
        public string? ColorCode { get; set; }

        /// <summary>
        /// Время начала смены
        /// </summary>
        public TimeOnly? Beginning { get; set; }

        /// <summary>
        /// Время окончания смены
        /// </summary>
        public TimeOnly? Ending { get; set; }

        /// <summary>
        /// Длительность смены в формате TimeSpan
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}
