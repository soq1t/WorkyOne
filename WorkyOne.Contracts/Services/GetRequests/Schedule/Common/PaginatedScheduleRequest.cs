using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.Contracts.Services.GetRequests.Schedule.Common
{
    /// <summary>
    /// Пагинированный запрос на получение информации о расписаниях из репозитория
    /// </summary>
    public sealed class PaginatedScheduleRequest : PaginatedRequest
    {
        [Required(ErrorMessage = "Укажите, нужно ли включать в запрос шаблоны, смены и т.д.")]
        public bool? IncludeFullData { get; set; }
    }
}
