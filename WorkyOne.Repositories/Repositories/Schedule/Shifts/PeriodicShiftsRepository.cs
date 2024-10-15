using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Common;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Репозиторий по работе с <see cref="PeriodicShiftEntity"/>
    /// </summary>
    public sealed class PeriodicShiftsRepository
        : EntityRepository<PeriodicShiftEntity, PeriodicShiftRequest>,
            IPeriodicShiftsRepository
    {
        public PeriodicShiftsRepository(IBaseRepository baseRepo, ApplicationDbContext context)
            : base(baseRepo, context) { }

        public Task<List<PeriodicShiftEntity>> GetByScheduleIdAsync(PeriodicShiftRequest request)
        {
            return _context
                .PeriodicShifts.Where(s => s.ScheduleId == request.ScheduleId)
                .ToListAsync();
        }
    }
}
