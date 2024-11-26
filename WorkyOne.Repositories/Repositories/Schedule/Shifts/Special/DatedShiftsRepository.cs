using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts.Special
{
    /// <summary>
    /// Репозиторий по работе с <see cref="DatedShiftEntity"/>
    /// </summary>
    public sealed class DatedShiftsRepository
        : ApplicationBaseRepository<
            ApplicationDbContext,
            DatedShiftEntity,
            EntityRequest<DatedShiftEntity>,
            PaginatedRequest<DatedShiftEntity>
        >,
            IDatedShiftsRepository
    {
        public DatedShiftsRepository(
            ApplicationDbContext context,
            IEntityUpdateUtility entityUpdater
        )
            : base(context, entityUpdater) { }

        public override Task<DatedShiftEntity?> GetAsync(
            EntityRequest<DatedShiftEntity> request,
            CancellationToken cancellation = default
        )
        {
            return QueryBuilder(request).FirstOrDefaultAsync(cancellation);
        }

        public override Task<List<DatedShiftEntity>> GetManyAsync(
            PaginatedRequest<DatedShiftEntity> request,
            CancellationToken cancellation = default
        )
        {
            return QueryBuilder(request).AddPagination(request).ToListAsync(cancellation);
        }

        private IQueryable<DatedShiftEntity> QueryBuilder(EntityRequest<DatedShiftEntity> request)
        {
            return _context
                .DatedShifts.Where(request.Specification.ToExpression())
                .Include(x => x.Shift);
        }
    }
}
