using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    public sealed class ShiftSequencesRepository
        : ApplicationBaseRepository<
            ApplicationDbContext,
            ShiftSequenceEntity,
            EntityRequest<ShiftSequenceEntity>,
            PaginatedRequest<ShiftSequenceEntity>
        >,
            IShiftSequencesRepository
    {
        public ShiftSequencesRepository(ApplicationDbContext context)
            : base(context) { }

        public override Task<ShiftSequenceEntity?> GetAsync(
            EntityRequest<ShiftSequenceEntity> request,
            CancellationToken cancellation = default
        )
        {
            var query = _context.ShiftSequences.Where(request.Specification.ToExpression());
            query = QueryBuilder(query);
            return query.FirstOrDefaultAsync(cancellation);
        }

        public override async Task<List<ShiftSequenceEntity>> GetManyAsync(
            PaginatedRequest<ShiftSequenceEntity> request,
            CancellationToken cancellation = default
        )
        {
            var query = _context.ShiftSequences.Where(request.Specification.ToExpression());
            query = QueryBuilder(query);
            query = query.AddPagination(request.PageIndex, request.Amount);

            var list = await query.ToListAsync(cancellation);
            return list.OrderBy(x => x.Position).ToList();
        }

        private IQueryable<ShiftSequenceEntity> QueryBuilder(IQueryable<ShiftSequenceEntity> query)
        {
            return query.Include(x => x.Shift);
        }
    }
}
