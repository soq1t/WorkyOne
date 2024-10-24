using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.Repositories.Requests.Common;

namespace WorkyOne.Contracts.Repositories.Requests.Schedule.Common
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
