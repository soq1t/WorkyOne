using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts.Special
{
    /// <summary>
    /// Репозиторий по работе с <see cref="TemplatedShiftEntity"/>
    /// </summary>
    public sealed class TemplatedShiftsRepository
        : ApplicationBaseRepository<
            ApplicationDbContext,
            TemplatedShiftEntity,
            EntityRequest<TemplatedShiftEntity>,
            PaginatedRequest<TemplatedShiftEntity>
        >,
            ITemplatedShiftsRepository
    {
        public TemplatedShiftsRepository(ApplicationDbContext context)
            : base(context) { }

        public override Task<TemplatedShiftEntity?> GetAsync(
            EntityRequest<TemplatedShiftEntity> request,
            CancellationToken cancellation = default
        )
        {
            return QueryBuilder(request).FirstOrDefaultAsync(cancellation);
        }

        public override Task<List<TemplatedShiftEntity>> GetManyAsync(
            PaginatedRequest<TemplatedShiftEntity> request,
            CancellationToken cancellation = default
        )
        {
            return QueryBuilder(request).AddPagination(request).ToListAsync(cancellation);
        }

        private IQueryable<TemplatedShiftEntity> QueryBuilder(
            EntityRequest<TemplatedShiftEntity> request
        )
        {
            return _context
                .TemplatedShifts.Where(request.Specification.ToExpression())
                .Include(x => x.Shift)
                .Include(x => x.Template)
                .OrderBy(x => x.Position);
        }
    }
}
