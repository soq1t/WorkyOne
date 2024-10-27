using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.DTOs.Schedule.Common
{
    /// <summary>
    /// DTO для DailyInfoEntity
    /// </summary>
    public sealed class DailyInfoDto : DtoBase
    {
        /// <summary>
        /// Название рабочего дня
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Указывает, является ли день рабочим
        /// </summary>
        [Required]
        public bool IsBusyDay { get; set; }

        /// <summary>
        /// Код цвета, которым выделяется данный день на графике
        /// </summary>
        public string ColorCode { get; set; }

        /// <summary>
        /// Дата, которую описывает сущность
        /// </summary>
        [Required]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Время начала рабочей смены
        /// </summary>
        public TimeOnly? Beginning { get; set; }

        /// <summary>
        /// Время окончания рабочей смены
        /// </summary>
        public TimeOnly? Ending { get; set; }

        /// <summary>
        /// Продолжительность рабочей смены
        /// </summary>
        public TimeSpan? ShiftProlongation { get; set; }
    }
}
