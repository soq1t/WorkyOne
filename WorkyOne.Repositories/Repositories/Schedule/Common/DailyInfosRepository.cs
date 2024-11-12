using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    public sealed class DailyInfosRepository
        : ApplicationBaseRepository<
            ApplicationDbContext,
            DailyInfoEntity,
            EntityRequest<DailyInfoEntity>,
            PaginatedRequest<DailyInfoEntity>
        >,
            IDailyInfosRepository
    {
        public DailyInfosRepository(ApplicationDbContext context)
            : base(context) { }

        public override Task<List<DailyInfoEntity>> GetManyAsync(
            PaginatedRequest<DailyInfoEntity> request,
            CancellationToken cancellation = default
        )
        {
            return _context
                .DailyInfos.Where(request.Specification.ToExpression())
                .OrderBy(x => x.Date)
                .AddPagination(request.PageIndex, request.Amount, true)
                .ToListAsync(cancellation);
        }
    }
}
