using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts.Basic;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts.Basic
{
    /// <summary>
    /// Репозиторий по работе с <see cref="PersonalShiftEntity"/>
    /// </summary>
    public class PersonalShiftRepository
        : ApplicationBaseRepository<
            ApplicationDbContext,
            PersonalShiftEntity,
            EntityRequest<PersonalShiftEntity>,
            PaginatedRequest<PersonalShiftEntity>
        >,
            IPersonalShiftRepository
    {
        public PersonalShiftRepository(ApplicationDbContext context)
            : base(context) { }

        public override Task<PersonalShiftEntity?> GetAsync(
            EntityRequest<PersonalShiftEntity> request,
            CancellationToken cancellation = default
        )
        {
            return _context
                .Shifts.OfType<PersonalShiftEntity>()
                .FirstOrDefaultAsync(request.Specification.ToExpression(), cancellation);
        }

        public override Task<List<PersonalShiftEntity>> GetManyAsync(
            PaginatedRequest<PersonalShiftEntity> request,
            CancellationToken cancellation = default
        )
        {
            var query = _context
                .Shifts.OfType<PersonalShiftEntity>()
                .Where(request.Specification.ToExpression());
            query = query.AddPagination(request.PageIndex, request.Amount);

            return query.ToListAsync(cancellation);
        }
    }
}
