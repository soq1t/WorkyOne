using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Entities.Abstractions.Common;
using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.Domain.Entities.Schedule.Common
{
    /// <summary>
    /// Сущность, описывающая расписание пользователя
    /// </summary>
    public sealed class ScheduleEntity : EntityBase
    {
        /// <summary>
        /// ID данных пользователя
        /// </summary>
        [Required]
        [ForeignKey(nameof(UserData))]
        public string UserDataId { get; set; }

        /// <summary>
        /// Данные пользователя, к которому относится расписание
        /// </summary>
        [Required]
        public UserDataEntity UserData { get; set; }

        /// <summary>
        /// Шаблон, который используется в текущем расписании
        /// </summary>
        public TemplateEntity? Template { get; set; }

        /// <summary>
        /// Название расписания
        /// </summary>
        [Required]
        [MaxLength(100)]
        [AutoUpdated]
        public string Name { get; set; }

        public List<PersonalShiftEntity> PersonalShifts { get; set; } = [];

        /// <summary>
        /// Список смен, выставляемых на конкретную дату
        /// </summary>
        public List<DatedShiftEntity> DatedShifts { get; set; } = new List<DatedShiftEntity>();

        /// <summary>
        /// Список смен, установленных на определённый период дней
        /// </summary>
        public List<PeriodicShiftEntity> PeriodicShifts { get; set; } =
            new List<PeriodicShiftEntity>();

        /// <summary>
        /// Рабочий график, сформированный по данному расписанию
        /// </summary>
        public List<DailyInfoEntity> Timetable { get; set; } = new List<DailyInfoEntity>();
    }
}
