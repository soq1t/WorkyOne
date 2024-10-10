using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Exceptions.Scedule;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Entities.Schedule.Shifts
{
    /// <summary>
    /// Абстрактный класс, описывающий рабочую смену
    /// </summary>
    public abstract class ShiftEntity : EntityBase, IUpdatable<ShiftEntity>
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
                    return duration;
                }
            }
        }

        public void UpdateFields(ShiftEntity entity)
        {
            Name = entity.Name;
            ColorCode = entity.ColorCode;
            Beginning = entity.Beginning;
            Ending = entity.Ending;
        }
    }
}
