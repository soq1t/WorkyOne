using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts.Basic
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="PersonalShiftEntity"/>
    /// </summary>
    public interface IPersonalShiftRepository
        : ICrudRepository<
            PersonalShiftEntity,
            EntityRequest<PersonalShiftEntity>,
            PaginatedRequest<PersonalShiftEntity>
        > { }
}
