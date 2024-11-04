using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Репозиторий по работе с <see cref="DatedShiftEntity"/>
    /// </summary>
    public sealed class DatedShiftsRepository
        : ApplicationBaseRepository<
            ApplicationDbContext,
            DatedShiftEntity,
            EntityRequest<DatedShiftEntity>,
            PaginatedRequest<DatedShiftEntity>
        >,
            IDatedShiftsRepository
    {
        public DatedShiftsRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
