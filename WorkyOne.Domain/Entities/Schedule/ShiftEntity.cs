using System.ComponentModel.DataAnnotations;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Exceptions.Scedule;

namespace WorkyOne.Domain.Entities.Schedule
{
    /// <summary>
    /// Сущность, описывающая рабочую смену
    /// </summary>
    public class ShiftEntity : EntityBase
    {
        /// <summary>
        /// Наименование смены
        /// </summary>
        [Required]
        [MaxLength(30)]
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
        /// Список повторений, в которых используется данная смена
        /// </summary>
        public List<RepititionEntity> Repetitions { get; set; } = new List<RepititionEntity>();

        /// <summary>
        /// Список дней, в которые эта смена используется отдельно от шаблона
        /// </summary>
        public List<SingleDayShiftEntity> SingleDayShifts { get; set; } =
            new List<SingleDayShiftEntity>();

        /// <summary>
        /// Список шаблонов, в которых используется данная схема
        /// </summary>

        public List<TemplateEntity> Templates { get; set; } = new List<TemplateEntity>();

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

        /// <summary>
        /// Является ли смена стандатной для приложения
        /// </summary>
        public bool IsPredefined = false;
    }
}
