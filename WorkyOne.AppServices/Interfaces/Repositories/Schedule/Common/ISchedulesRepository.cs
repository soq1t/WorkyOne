using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="ScheduleEntity"/>
    /// </summary>
    public interface ISchedulesRepository : IEntityRepository<ScheduleEntity, ScheduleRequest>
    {
        /// <summary>
        /// Возвращает список расписаний для указанного в запросе пользователя
        /// </summary>
        /// <param name="request">Запрос на получение данных из базы</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<ICollection<ScheduleEntity>> GetByUserAsync(
            ScheduleRequest request,
            CancellationToken cancellation = default
        );
    }
}
