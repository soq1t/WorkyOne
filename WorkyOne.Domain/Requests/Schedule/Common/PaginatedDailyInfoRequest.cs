using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Schedule.Common
{
    /// <inheritdoc/>
    public sealed class PaginatedDailyInfoRequest : PaginatedRequest<DailyInfoEntity>
    {
        /// <summary>
        /// Создаёт запрос на получение <see cref="DailyInfoEntity"/> за определённый период для определённого <see cref="ScheduleEntity"/>
        /// </summary>
        /// <param name="startDate">Дата, с которой начинается выборка <see cref="DailyInfoEntity"/></param>
        /// <param name="endDate">Дата, которой оканчивается выборка <see cref="DailyInfoEntity"/></param>
        /// <param name="scheduleId">Идентификатор расписания</param>
        public PaginatedDailyInfoRequest(DateOnly startDate, DateOnly endDate, string scheduleId)
        {
            var days = endDate.DayNumber - startDate.DayNumber + 1;
            PageIndex = 1;
            Amount = days;
            Predicate = (x) =>
                x.ScheduleId == scheduleId && x.Date >= startDate && x.Date <= endDate;
        }
    }
}
