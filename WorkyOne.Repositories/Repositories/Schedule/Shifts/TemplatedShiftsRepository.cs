using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Репозиторий по работе с <see cref="TemplatedShiftEntity"/>
    /// </summary>
    public sealed class TemplatedShiftsRepository
        : ApplicationBaseRepository<
            TemplatedShiftEntity,
            EntityRequest<TemplatedShiftEntity>,
            PaginatedTemplatedShiftRequest
        >,
            ITemplatedShiftsRepository
    {
        public TemplatedShiftsRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
