using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;

namespace WorkyOne.Contracts.DTOs.Schedule.Common
{
    /// <summary>
    /// DTO информации о расписании
    /// </summary>
    public class ScheduleDto : DtoBase
    {
        /// <summary>
        /// ID данных пользователя
        /// </summary>
        [Required]
        public string UserDataId { get; set; }

        /// <summary>
        /// Название расписания
        /// </summary>
        [Required(ErrorMessage = "Укажите название расписания")]
        [MaxLength(100)]
        [DisplayName("Название расписания")]
        public string Name { get; set; }

        /// <summary>
        /// Шаблон, который используется в текущем расписании
        /// </summary>
        [Required]
        public TemplateDto Template { get; set; } = new TemplateDto();

        /// <summary>
        /// Список смен, используемых в текущем расписании
        /// </summary>
        public List<PersonalShiftDto> PersonalShifts { get; set; } = [];

        /// <summary>
        /// Список смен, используемых во всём приложении
        /// </summary>
        public List<SharedShiftDto> SharedShifts { get; set; } = [];

        /// <summary>
        /// Список смен, выставляемых на конкретную дату
        /// </summary>
        public List<DatedShiftDto> DatedShifts { get; set; } = new List<DatedShiftDto>();

        /// <summary>
        /// Список смен, установленных на определённый период дней
        /// </summary>
        public List<PeriodicShiftDto> PeriodicShifts { get; set; } = new List<PeriodicShiftDto>();
    }
}
