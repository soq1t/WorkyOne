using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Schedule.Common;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Репозиторий по работе с <see cref="DatedShiftEntity"/>
    /// </summary>
    public sealed class DatedShiftsRepository
        : EntityRepository<DatedShiftEntity, DatedShiftRequest>,
            IDatedShiftsRepository
    {
        public DatedShiftsRepository(IBaseRepository baseRepo, ApplicationDbContext context)
            : base(baseRepo, context) { }

        public Task<List<DatedShiftEntity>> GetByScheduleIdAsync(DatedShiftRequest request)
        {
            return _context
                .DatedShifts.Where(s => s.ScheduleId == request.ScheduleId)
                .ToListAsync();
        }
    }
}
