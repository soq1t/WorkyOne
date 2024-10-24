using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.Attributes.Validation;
using WorkyOne.Contracts.Enums.Attributes;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.Contracts.Services.GetRequests.Schedule
{
    /// <summary>
    /// Запрос на получение рабочего графика
    /// </summary>
    public sealed class PaginatedWorkGraphicRequest : PaginatedRequest
    {
        /// <summary>
        /// Идентификатор рабочего графика
        /// </summary>
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
