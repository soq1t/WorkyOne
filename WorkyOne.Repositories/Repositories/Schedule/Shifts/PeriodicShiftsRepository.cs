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
    /// Репозиторий по работе с <see cref="PeriodicShiftEntity"/>
    /// </summary>
    public sealed class PeriodicShiftsRepository : IPeriodicShiftsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseRepository _baseRepo;

        public PeriodicShiftsRepository(ApplicationDbContext context, IBaseRepository baseRepo)
        {
            _context = context;
            _baseRepo = baseRepo;
        }

        public Task<RepositoryResult> CreateAsync(PeriodicShiftEntity entity)
        {
            return _baseRepo.CreateAsync(entity);
        }

        public Task<RepositoryResult> CreateManyAsync(ICollection<PeriodicShiftEntity> entities)
        {
            return _baseRepo.CreateManyAsync(entities);
        }

        public Task<RepositoryResult> DeleteAsync(string entityId)
        {
            return _baseRepo.DeleteAsync<PeriodicShiftEntity>(entityId);
        }

        public Task<RepositoryResult> DeleteManyAsync(ICollection<string> entityIds)
        {
            return _baseRepo.DeleteManyAsync<PeriodicShiftEntity>(entityIds);
        }

        public Task<PeriodicShiftEntity?> GetAsync(PeriodicShiftRequest request)
        {
            return _context.PeriodicShifts.FirstOrDefaultAsync(s => s.Id == request.Id);
        }

        public Task<List<PeriodicShiftEntity>> GetByScheduleIdAsync(PeriodicShiftRequest request)
        {
            return _context
                .PeriodicShifts.Where(s => s.ScheduleId == request.ScheduleId)
                .ToListAsync();
        }

        public Task<RepositoryResult> RenewAsync(
            ICollection<PeriodicShiftEntity> oldEntities,
            ICollection<PeriodicShiftEntity> newEntities
        )
        {
            return DefaultRepositoryMethods.RenewAsync(this, oldEntities, newEntities);
        }

        public Task<RepositoryResult> UpdateAsync(PeriodicShiftEntity entity)
        {
            return DefaultRepositoryMethods.UpdateAsync(_context, entity);
        }

        public Task<RepositoryResult> UpdateManyAsync(ICollection<PeriodicShiftEntity> entities)
        {
            return DefaultRepositoryMethods.UpdateManyAsync(_context, entities);
        }
    }
}
