using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
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
            Expression<Func<TemplateEntity, bool>>? predicate =
                (request.EntityId != null) ? (x) => x.Id == request.EntityId : request.Predicate;

            if (predicate == null)
            {
                return Task.FromResult<TemplateEntity?>(null);
            }

            return _context
                .Templates.Where(predicate)
                .Include(t => t.Shifts)
                .Include(t => t.Sequences)
                .FirstOrDefaultAsync();
        }

        public override Task<List<TemplateEntity>> GetManyAsync(
            PaginatedRequest<TemplateEntity> request,
            CancellationToken cancellation = default
        )
        {
            var skip = (request.PageIndex - 1) * request.Amount;
            var take = request.Amount;

            return _context
                .Templates.Where(request.Predicate)
                .Include(t => t.Shifts)
                .Include(t => t.Sequences)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellation);
        }
    }
}
