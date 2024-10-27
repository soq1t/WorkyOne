using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Contracts.Repositories.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    public sealed class DailyInfosRepository
        : ApplicationBaseRepository<
            DailyInfoEntity,
            EntityRequest<DailyInfoEntity>,
            PaginatedRequest<DailyInfoEntity>
        >,
            IDailyInfosRepository
    {
        public DailyInfosRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
