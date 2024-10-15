using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Common;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Репозиторий по работе с <see cref="TemplatedShiftEntity"/>
    /// </summary>
    public sealed class TemplatedShiftsRepository
        : EntityRepository<TemplatedShiftEntity, TemplatedShiftRequest>,
            ITemplatedShiftsRepository
    {
        public TemplatedShiftsRepository(IBaseRepository baseRepo, ApplicationDbContext context)
            : base(baseRepo, context) { }

        public async Task<ICollection<TemplatedShiftEntity>> GetByTemplateIdAsync(
            TemplatedShiftRequest request
        )
        {
            return await _context
                .TemplatedShifts.Where(s => s.TemplateId == request.TemplateId)
                .ToListAsync();
        }
    }
}
