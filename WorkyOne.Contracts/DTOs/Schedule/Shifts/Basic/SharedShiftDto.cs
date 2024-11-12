using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic
{
    /// <summary>
    /// DTO смены, используемой во всех расписаниях
    /// </summary>
    public class SharedShiftDto : DtoBase
    {
        /// <summary>
        /// Название смены
        /// </summary>
        [Required]
        [Length(1, 30)]
        public string Name { get; set; } = "Смена";

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
        public string? ColorCode { get; set; } = "#FFFFFF";

        /// <summary>
        /// Время начала смены
        /// </summary>
        public TimeOnly? Beginning { get; set; }

        /// <summary>
        /// Время окончания смены
        /// </summary>
        public TimeOnly? Ending { get; set; }

        /// <summary>
        /// Возвращает длительность смены в формате TimeSpan
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}
