using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="TemplatedShiftEntity"/>
    /// </summary>
    public interface ITemplatedShiftsRepository
        : ICrudRepository<
            TemplatedShiftEntity,
            EntityRequest<TemplatedShiftEntity>,
            PaginatedRequest<TemplatedShiftEntity>
        > { }
}
