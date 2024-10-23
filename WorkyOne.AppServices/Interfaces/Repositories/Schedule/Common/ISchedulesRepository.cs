using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="ScheduleEntity"/>
    /// </summary>
    public interface ISchedulesRepository
        : ICrudRepository<ScheduleEntity, ScheduleRequest, PaginatedScheduleRequest> { }
}
