﻿using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;

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
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Шаблон, который используется в текущем расписании
        /// </summary>
        public TemplateDto? Template { get; set; }

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
