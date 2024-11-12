using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts.Basic
{
    /// <summary>
    /// Интерфейс репозитория для работы с <see cref="SharedShiftEntity"/>
    /// </summary>
    public interface ISharedShiftsRepository
        : ICrudRepository<
            SharedShiftEntity,
            EntityRequest<SharedShiftEntity>,
            PaginatedRequest<SharedShiftEntity>
        > { }
}
