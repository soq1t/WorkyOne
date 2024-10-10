using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Utilities;

namespace WorkyOne.Repositories.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Репозиторий по работе с <see cref="TemplatedShiftEntity"/>
    /// </summary>
    public sealed class TemplatedShiftsRepository : ITemplatedShiftsRepository
    {
        private readonly IBaseRepository _baseRepo;

        private readonly ApplicationDbContext _context;

        public TemplatedShiftsRepository(
            ApplicationDbContext context,
            IBaseRepository baseRepository
        )
        {
            _context = context;
            _baseRepo = baseRepository;
        }

        public Task<RepositoryResult> CreateAsync(TemplatedShiftEntity entity)
        {
            return _baseRepo.CreateAsync(entity);
        }

        public Task<RepositoryResult> CreateManyAsync(ICollection<TemplatedShiftEntity> entities)
        {
            return _baseRepo.CreateManyAsync(entities);
        }

        public Task<RepositoryResult> DeleteAsync(string entityId)
        {
            return _baseRepo.DeleteAsync<TemplatedShiftEntity>(entityId);
        }

        public Task<RepositoryResult> DeleteManyAsync(ICollection<string> entityIds)
        {
            return _baseRepo.DeleteManyAsync<TemplatedShiftEntity>(entityIds);
        }

        public Task<TemplatedShiftEntity?> GetAsync(TemplatedShiftRequest request)
        {
            return _context.TemplatedShifts.FirstOrDefaultAsync(e => e.Id == request.Id);
        }

        public Task<ICollection<TemplatedShiftEntity>> GetByTemplateIdAsync(
            TemplatedShiftRequest request
        )
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResult> RenewAsync(
            ICollection<TemplatedShiftEntity> oldEntities,
            ICollection<TemplatedShiftEntity> newEntities
        )
        {
            return DefaultRepositoryMethods.RenewAsync(this, oldEntities, newEntities);
        }

        public async Task<RepositoryResult> UpdateAsync(TemplatedShiftEntity entity)
        {
            var updated = await _context.TemplatedShifts.FirstOrDefaultAsync(s =>
                s.Id == entity.Id
            );

            if (updated == null)
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
            }

            updated.UpdateFields(entity);
            _context.Update(updated);
            await _context.SaveChangesAsync();
            return new RepositoryResult(entity.Id);
        }

        public async Task<RepositoryResult> UpdateManyAsync(
            ICollection<TemplatedShiftEntity> entities
        )
        {
            var ids = entities.Select(s => s.TemplateId).ToList();

            var updated = await _context
                .TemplatedShifts.Where(s => ids.Contains(s.Id))
                .ToListAsync();

            var updatedIds = updated.Select(x => x.Id);

            var notExistedIds = ids.Except(updatedIds).ToList();

            if (updated.Count == 0)
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists);
            }

            var result = new RepositoryResult();
            notExistedIds.ForEach(id => result.AddError(RepositoryErrorType.EntityNotExists, id));
            result.SucceedIds.AddRange(updatedIds);

            foreach (var entity in updated)
            {
                entity.UpdateFields(entities.First(e => e.Id == entity.Id));
                _context.Update(entity);
            }

            await _context.SaveChangesAsync();
            return result;
        }
    }
}
