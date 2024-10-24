using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.Attributes.Validation;
using WorkyOne.Contracts.Enums.Attributes;

namespace WorkyOne.Contracts.Repositories.Requests.Common
{
    /// <summary>
    /// Запрос на проведение манипуляций с рабочим графиком согласно указанному расписанию
    /// </summary>
    public sealed class WorkGraphicRequest : PaginatedRequest
    {
        /// <summary>
        /// Идентификатор расписания
        /// </summary>
        [Required(ErrorMessage = "Введите идентификатор расписания")]
        public string? ScheduleId { get; set; }

        /// <summary>
        /// Дата начала рабочего графика
        /// </summary>
        [Required(ErrorMessage = "Введите дату, с которой начинается рабочий график")]
        [DateCompare(DateCompareMode.LessOrEquial, nameof(EndDate))]
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// Дата окончания рабочего графика
        /// </summary>
        [Required(ErrorMessage = "Введите дату, которой оканчивается рабочий график")]
        [DateCompare(DateCompareMode.MoreOrEquial, nameof(StartDate))]
        public DateOnly? EndDate { get; set; }
    }
}
