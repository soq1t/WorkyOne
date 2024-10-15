using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="DailyInfoEntity"/>
    /// </summary>
    public interface IDailyInfosRepository : IEntityRepository<DailyInfoEntity, DailyInfoRequest>
    {
        /// <summary>
        /// Возвращает множество <see cref="DailyInfoEntity"/> для указанного <see cref="ScheduleEntity"/>
        /// </summary>
        /// <param name="request">Запрос на получение данных о <see cref="DailyInfoEntity"/></param>
        /// <returns></returns>
        public Task<List<DailyInfoEntity>> GetByScheduleIdAsync(DailyInfoRequest request);
    }
}
