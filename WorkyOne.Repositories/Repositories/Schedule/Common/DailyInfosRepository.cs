using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Common;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    public sealed class DailyInfosRepository
        : EntityRepository<DailyInfoEntity, DailyInfoRequest>,
            IDailyInfosRepository
    {
        public DailyInfosRepository(IBaseRepository baseRepo, ApplicationDbContext context)
            : base(baseRepo, context) { }

        public Task<List<DailyInfoEntity>> GetByScheduleIdAsync(DailyInfoRequest request)
        {
            return _context.DailyInfos.Where(x => x.ScheduleId == request.ScheduleId).ToListAsync();
        }
    }
}
