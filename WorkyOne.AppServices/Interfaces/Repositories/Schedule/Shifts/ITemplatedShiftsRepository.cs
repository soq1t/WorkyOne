using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="TemplatedShiftEntity"/>
    /// </summary>
    public interface ITemplatedShiftsRepository
        : IEntityRepository<TemplatedShiftEntity, TemplatedShiftRequest>
    {
        /// <summary>
        /// Возвращает список смен для указанного <see cref="TemplateEntity"/>
        /// </summary>
        /// <param name="request">Запрос на получение списка <see cref="TemplatedShiftEntity"/></param>
        /// <param name="cancellation">Токен отмены задания</param>

        public Task<ICollection<TemplatedShiftEntity>> GetByTemplateIdAsync(
            TemplatedShiftRequest request,
            CancellationToken cancellation = default
        );
    }
}
