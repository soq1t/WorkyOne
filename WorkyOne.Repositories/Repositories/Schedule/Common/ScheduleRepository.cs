using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Interfaces.Requests.Schedule;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    /// <summary>
    /// Репозиторий по работе с <see cref="ScheduleEntity"/>
    /// </summary>
    public sealed class ScheduleRepository
        : ApplicationBaseRepository<ScheduleEntity, ScheduleRequest, PaginatedScheduleRequest>,
            ISchedulesRepository
    {
        public ScheduleRepository(ApplicationDbContext context)
            : base(context) { }

        public override Task<ScheduleEntity?> GetAsync(
            ScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            IQueryable<ScheduleEntity> query = _context.Schedules.Where(s =>
                s.Id == request.EntityId
            );
            query = QueryBuilder(request, query);

            return query.FirstOrDefaultAsync(cancellation);
        }

        public override Task<List<ScheduleEntity>> GetManyAsync(
            PaginatedScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            IQueryable<ScheduleEntity> query = _context.Schedules.Where(request.Predicate);
            query = QueryBuilder(request, query);
            query = AddPagination(query, request.PageIndex, request.Amount);

            return query.ToListAsync(cancellation);
        }

        private IQueryable<ScheduleEntity> QueryBuilder(
            IScheduleRequest request,
            IQueryable<ScheduleEntity> query
        )
        {
            if (request.IncludeTemplate)
            {
                query = query.Include(s => s.Template).ThenInclude(t => t.Sequences);
                query = query.Include(s => s.Template).ThenInclude(t => t.Shifts);
            }

            if (request.IncludeShifts)
            {
                query = query.Include(s => s.PeriodicShifts);
                query = query.Include(s => s.DatedShifts);
            }

            return query;
        }
    }
}
