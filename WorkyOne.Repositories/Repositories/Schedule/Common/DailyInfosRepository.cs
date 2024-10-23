using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Contracts.Repositories.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    public sealed class DailyInfosRepository
        : ApplicationBaseRepository<
            DailyInfoEntity,
            EntityRequest<DailyInfoEntity>,
            PaginatedDailyInfoRequest
        >,
            IDailyInfosRepository
    {
        public DailyInfosRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<RepositoryResult> DeleteByConditionAsync(
            Func<DailyInfoEntity, bool> predicate,
            CancellationToken cancellation = default
        )
        {
            await _context.DailyInfos.Where(e => predicate(e)).ExecuteDeleteAsync(cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancelationRequested();
            }
            else
            {
                return new RepositoryResult("1");
            }
        }
    }
}
