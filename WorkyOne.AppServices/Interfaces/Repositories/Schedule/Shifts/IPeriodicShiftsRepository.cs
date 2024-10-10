using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="PeriodicShiftEntity"/>
    /// </summary>
    public interface IPeriodicShiftsRepository
        : IEntityRepository<PeriodicShiftEntity, PeriodicShiftRequest>
    {
        /// <summary>
        /// Возвращает список "перидичных" смен для указанного <see cref="ScheduleEntity"/>
        /// </summary>
        /// <param name="request">Запрос на получение данных из базы</param>
        /// <returns></returns>
        public Task<List<PeriodicShiftEntity>> GetByScheduleIdAsync(PeriodicShiftRequest request);
    }
}
