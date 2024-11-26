using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    public sealed class TemplatesRepository
        : ApplicationBaseRepository<
            ApplicationDbContext,
            TemplateEntity,
            EntityRequest<TemplateEntity>,
            PaginatedRequest<TemplateEntity>
        >,
            ITemplatesRepository
    {
        public TemplatesRepository(ApplicationDbContext context, IEntityUpdateUtility entityUpdater)
            : base(context, entityUpdater) { }

        public override Task<TemplateEntity?> GetAsync(
            EntityRequest<TemplateEntity> request,
            CancellationToken cancellation = default
        )
        {
            return QueryBuilder(request).FirstOrDefaultAsync(cancellation);
        }

        public override Task<List<TemplateEntity>> GetManyAsync(
            PaginatedRequest<TemplateEntity> request,
            CancellationToken cancellation = default
        )
        {
            return QueryBuilder(request).AddPagination(request).ToListAsync(cancellation);
        }

        private IQueryable<TemplateEntity> QueryBuilder(EntityRequest<TemplateEntity> request)
        {
            return _context
                .Templates.Where(request.Specification.ToExpression())
                .Include(x => x.Shifts)
                .ThenInclude(x => x.Shift)
                .Include(x => x.Schedule);
        }
    }
}
