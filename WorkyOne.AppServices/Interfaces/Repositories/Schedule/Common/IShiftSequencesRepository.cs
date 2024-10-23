using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="ShiftSequenceEntity"/>
    /// </summary>
    public interface IShiftSequencesRepository
        : ICrudRepository<
            ShiftSequenceEntity,
            EntityRequest<ShiftSequenceEntity>,
            PaginatedShiftSequencesRequest
        > { }
}
