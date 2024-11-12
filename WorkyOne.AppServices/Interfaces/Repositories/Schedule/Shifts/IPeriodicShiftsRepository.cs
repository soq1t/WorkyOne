using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="PeriodicShiftEntity"/>
    /// </summary>
    public interface IPeriodicShiftsRepository
        : ICrudRepository<
            PeriodicShiftEntity,
            EntityRequest<PeriodicShiftEntity>,
            PaginatedRequest<PeriodicShiftEntity>
        > { }
}
