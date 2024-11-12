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
    /// Репозиторий по работе с <see cref="PeriodicShiftEntity"/>
    /// </summary>
    public sealed class PeriodicShiftsRepository
        : ApplicationBaseRepository<
            ApplicationDbContext,
            PeriodicShiftEntity,
            EntityRequest<PeriodicShiftEntity>,
            PaginatedRequest<PeriodicShiftEntity>
        >,
            IPeriodicShiftsRepository
    {
        public PeriodicShiftsRepository(ApplicationDbContext context)
            : base(context) { }

        public override Task<PeriodicShiftEntity?> GetAsync(
            EntityRequest<PeriodicShiftEntity> request,
            CancellationToken cancellation = default
        )
        {
            return QueryBuilder(request).FirstOrDefaultAsync(cancellation);
        }

        public override Task<List<PeriodicShiftEntity>> GetManyAsync(
            PaginatedRequest<PeriodicShiftEntity> request,
            CancellationToken cancellation = default
        )
        {
            return QueryBuilder(request).AddPagination(request).ToListAsync(cancellation);
        }

        private IQueryable<PeriodicShiftEntity> QueryBuilder(
            EntityRequest<PeriodicShiftEntity> request
        )
        {
            return _context
                .PeriodicShifts.Where(request.Specification.ToExpression())
                .Include(x => x.Shift);
        }
    }
}
