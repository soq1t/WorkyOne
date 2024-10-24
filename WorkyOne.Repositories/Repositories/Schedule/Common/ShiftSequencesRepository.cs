using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Contracts.Repositories.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    public sealed class ShiftSequencesRepository
        : ApplicationBaseRepository<
            ShiftSequenceEntity,
            EntityRequest<ShiftSequenceEntity>,
            PaginatedShiftSequencesRequest
        >,
            IShiftSequencesRepository
    {
        public ShiftSequencesRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<RepositoryResult> DeleteByConditionAsync(
            Expression<Func<ShiftSequenceEntity, bool>> predicate,
            CancellationToken cancellation = default
        )
        {
            await _context.ShiftSequences.Where(predicate).ExecuteDeleteAsync(cancellation);

            return RepositoryResult.Ok("1");
        }
    }
}
