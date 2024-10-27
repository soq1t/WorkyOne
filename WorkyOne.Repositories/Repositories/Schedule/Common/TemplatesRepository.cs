using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    public sealed class TemplatesRepository
        : ApplicationBaseRepository<
            TemplateEntity,
            EntityRequest<TemplateEntity>,
            PaginatedRequest<TemplateEntity>
        >,
            ITemplatesRepository
    {
        public TemplatesRepository(ApplicationDbContext context)
            : base(context) { }

        public override Task<TemplateEntity?> GetAsync(
            EntityRequest<TemplateEntity> request,
            CancellationToken cancellation = default
        )
        {
            var query = _context.Templates.Where(request.Specification.ToExpression());
            query = QueryBuilder(query);
            return query.FirstOrDefaultAsync(cancellation);
        }

        public override Task<List<TemplateEntity>> GetManyAsync(
            PaginatedRequest<TemplateEntity> request,
            CancellationToken cancellation = default
        )
        {
            var query = _context.Templates.Where(request.Specification.ToExpression());
            query = query.AddPagination(request.PageIndex, request.Amount);
            query = QueryBuilder(query);
            return query.ToListAsync(cancellation);
        }

        private static IQueryable<TemplateEntity> QueryBuilder(IQueryable<TemplateEntity> query)
        {
            query = query.Include(t => t.Shifts).Include(t => t.Sequences);
            return query;
        }
    }
}
