using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Репозиторий по работе с <see cref="PeriodicShiftEntity"/>
    /// </summary>
    public sealed class PeriodicShiftsRepository
        : ApplicationBaseRepository<
            PeriodicShiftEntity,
            EntityRequest<PeriodicShiftEntity>,
            PaginatedPeriodicShiftRequest
        >,
            IPeriodicShiftsRepository
    {
        public PeriodicShiftsRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
