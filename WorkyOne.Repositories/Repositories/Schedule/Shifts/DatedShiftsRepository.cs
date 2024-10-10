using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Utilities;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Репозиторий по работе с <see cref="DatedShiftEntity"/>
    /// </summary>
    public sealed class DatedShiftsRepository : IDatedShiftsRepository
    {
        private readonly IBaseRepository _baseRepo;
        private readonly ApplicationDbContext _context;

        public DatedShiftsRepository(IBaseRepository baseRepo, ApplicationDbContext context)
        {
            _baseRepo = baseRepo;
            _context = context;
        }

        public Task<RepositoryResult> CreateAsync(DatedShiftEntity entity)
        {
            return _baseRepo.CreateAsync(entity);
        }

        public Task<RepositoryResult> CreateManyAsync(ICollection<DatedShiftEntity> entities)
        {
            return _baseRepo.CreateManyAsync(entities);
        }

        public Task<RepositoryResult> DeleteAsync(string entityId)
        {
            return _baseRepo.DeleteAsync<DatedShiftEntity>(entityId);
        }

        public Task<RepositoryResult> DeleteManyAsync(ICollection<string> entityIds)
        {
            return _baseRepo.DeleteManyAsync<DatedShiftEntity>(entityIds);
        }

        public Task<DatedShiftEntity?> GetAsync(DatedShiftRequest request)
        {
            return _context.DatedShifts.FirstOrDefaultAsync(s => s.Id == request.Id);
        }

        public Task<List<DatedShiftEntity>> GetByScheduleIdAsync(DatedShiftRequest request)
        {
            return _context
                .DatedShifts.Where(s => s.ScheduleId == request.ScheduleId)
                .ToListAsync();
        }

        public Task<RepositoryResult> RenewAsync(
            ICollection<DatedShiftEntity> oldEntities,
            ICollection<DatedShiftEntity> newEntities
        )
        {
            return DefaultRepositoryMethods.RenewAsync(this, oldEntities, newEntities);
        }

        public Task<RepositoryResult> UpdateAsync(DatedShiftEntity entity)
        {
            return DefaultRepositoryMethods.UpdateAsync(_context, entity);
        }

        public Task<RepositoryResult> UpdateManyAsync(ICollection<DatedShiftEntity> entities)
        {
            return DefaultRepositoryMethods.UpdateManyAsync(_context, entities);
        }
    }
}
