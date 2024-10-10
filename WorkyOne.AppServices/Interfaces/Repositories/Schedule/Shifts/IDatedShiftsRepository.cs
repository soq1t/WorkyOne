using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="DatedShiftEntity"/>
    /// </summary>
    public interface IDatedShiftsRepository : IEntityRepository<DatedShiftEntity, DatedShiftRequest>
    {
        /// <summary>
        /// Возвращает все "датированные" смены для указанного <see cref="ScheduleEntity"/>
        /// </summary>
        /// <param name="request">Запрос на получение данных</param>
        /// <returns></returns>
        public Task<ICollection<DatedShiftEntity>> GetByScheduleIdAsync(DatedShiftRequest request);
    }
}
