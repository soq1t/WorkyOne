using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Репозиторий для работы с <see cref="SharedShiftEntity"/>
    /// </summary>
    public class SharedShiftsRepository
        : ApplicationBaseRepository<
            ApplicationDbContext,
            SharedShiftEntity,
            EntityRequest<SharedShiftEntity>,
            PaginatedRequest<SharedShiftEntity>
        >,
            ISharedShiftsRepository
    {
        public SharedShiftsRepository(ApplicationDbContext context)
            : base(context) { }

        public override Task<SharedShiftEntity?> GetAsync(
            EntityRequest<SharedShiftEntity> request,
            CancellationToken cancellation = default
        )
        {
            return _context
                .Shifts.OfType<SharedShiftEntity>()
                .FirstOrDefaultAsync(request.Specification.ToExpression(), cancellation);
        }

        public override Task<List<SharedShiftEntity>> GetManyAsync(
            PaginatedRequest<SharedShiftEntity> request,
            CancellationToken cancellation = default
        )
        {
            var query = _context
                .Shifts.OfType<SharedShiftEntity>()
                .Where(request.Specification.ToExpression());

            query = query.AddPagination(request.PageIndex, request.Amount);

            return query.ToListAsync(cancellation);
        }
    }
}
