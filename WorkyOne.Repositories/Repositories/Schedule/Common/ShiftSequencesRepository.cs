using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Repositories.Contextes;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    /// <summary>
    /// Репозиторий по работе с <see cref="ShiftSequenceEntity"/>
    /// </summary>
    public sealed class ShiftSequencesRepository
        : EntityRepository<ShiftSequenceEntity, ShiftSequenceRequest>,
            IShiftSequencesRepository
    {
        public ShiftSequencesRepository(IBaseRepository baseRepo, ApplicationDbContext context)
            : base(baseRepo, context) { }

        /// <inheritdoc/>
        public async Task<ICollection<ShiftSequenceEntity>> GetByTemplateIdAsync(
            ShiftSequenceRequest request
        )
        {
            return await _context
                .ShiftSequences.Where(s => s.TemplateId == request.TemplateId)
                .ToListAsync();
        }
    }
}
