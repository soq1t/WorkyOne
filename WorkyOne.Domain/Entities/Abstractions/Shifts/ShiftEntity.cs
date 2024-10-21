using System.ComponentModel.DataAnnotations;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Entities.Abstractions.Common;
using WorkyOne.Domain.Exceptions.Scedule;

namespace WorkyOne.Domain.Entities.Abstractions.Shifts
{
    /// <summary>
    /// Абстрактный класс, описывающий рабочую смену
    /// </summary>
    public abstract class ShiftEntity : EntityBase
    {
        /// <summary>
        /// Название смены
        /// </summary>
        [Required]
        [Length(1, 30)]
        [AutoUpdated]
        public string Name { get; set; } = "Смена";

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
        [AutoUpdated]
        public string? ColorCode { get; set; } = "#FFFFFF";

        /// <summary>
        /// Время начала смены
        /// </summary>
        [AutoUpdated]
        public TimeOnly? Beginning { get; set; }

        /// <summary>
        /// Время окончания смены
        /// </summary>
        [AutoUpdated]
        public TimeOnly? Ending { get; set; }

        /// <summary>
        /// Возвращает длительность смены в формате TimeSpan
        /// </summary>
        public TimeSpan Duration()
        {
            if (Beginning == null && Ending == null)
            {
                return TimeSpan.Zero;
            }
            else if (Beginning == null || Ending == null)
            {
                throw new WrongShiftTimeException(Beginning != null);
            }
            else
            {
                if (Beginning <= Ending)
                {
                    TimeSpan duration = Ending.Value - Beginning.Value;
                    return duration;
                }
                else
                {
                    TimeSpan duration = TimeOnly.MaxValue - Beginning.Value;
                    duration += Ending.Value.ToTimeSpan();
                    duration =
                        new TimeSpan(duration.Hours, duration.Minutes, 0) + new TimeSpan(0, 1, 0);
                    return duration;
                }
            }
        }
    }
}
