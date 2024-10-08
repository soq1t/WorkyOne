using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Utilities;

namespace WorkyOne.Repositories.Repositories.Schedule
{
    /// <summary>
    /// Репозиторий по работе с <see cref="TemplatedShiftEntity"/>
    /// </summary>
    public sealed class TemplatedShiftsRepository
        : IEntityRepository<TemplatedShiftEntity, TemplatedShiftRequest>
    {
        private readonly ApplicationDbContext _context;

        public TemplatedShiftsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResult> CreateAsync(TemplatedShiftEntity entity)
        {
            bool entityExists = await _context.TemplatedShifts.AnyAsync(s => s.Id == entity.Id);

            if (entityExists)
            {
                return new RepositoryResult(RepositoryErrorType.EntityAlreadyExists, entity.Id);
            }

            _context.TemplatedShifts.Add(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult(entity.Id);
        }

        public async Task<RepositoryResult> CreateManyAsync(
            ICollection<TemplatedShiftEntity> entities
        )
        {
            List<string> entitiesIds = entities.Select(e => e.Id).ToList();

            List<string> existedEntitiesIds = await _context
                .TemplatedShifts.Where(s => entitiesIds.Contains(s.Id))
                .Select(s => s.Id)
                .ToListAsync();

            if (existedEntitiesIds.Count == entities.Count)
            {
                return new RepositoryResult(RepositoryErrorType.EntityAlreadyExists);
            }

            var result = new RepositoryResult();
            foreach (
                TemplatedShiftEntity entity in entities.Where(e =>
                    !existedEntitiesIds.Contains(e.Id)
                )
            )
            {
                result.SucceedIds.Add(entity.Id);
                _context.TemplatedShifts.Add(entity);
            }

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<RepositoryResult> DeleteAsync(string entityId)
        {
            var deleted = await _context.TemplatedShifts.FirstOrDefaultAsync(s => s.Id == entityId);

            if (deleted == null)
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists, entityId);
            }

            _context.Remove(deleted);
            await _context.SaveChangesAsync();
            return new RepositoryResult(entityId);
        }

        public async Task<RepositoryResult> DeleteManyAsync(ICollection<string> entityIds)
        {
            var deleted = await _context
                .TemplatedShifts.Where(s => entityIds.Contains(s.Id))
                .ToListAsync();

            if (deleted.Count == 0)
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists);
            }

            _context.RemoveRange(deleted);
            await _context.SaveChangesAsync();
            return new RepositoryResult(deleted.Select(e => e.Id));
        }

        public Task<TemplatedShiftEntity?> GetAsync(TemplatedShiftRequest request)
        {
            return _context.TemplatedShifts.FirstOrDefaultAsync(e => e.Id == request.Id);
        }

        public async Task<ICollection<TemplatedShiftEntity>?> GetManyAsync(
            TemplatedShiftRequest request
        )
        {
            return await _context
                .TemplatedShifts.Where(s => s.TemplateId == request.TemplateId)
                .ToListAsync();
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
            var updatedIds = entities.Select(s => s.TemplateId).ToList();
            var updated = await _context
                .TemplatedShifts.Where(s => updatedIds.Contains(s.Id))
                .ToListAsync();

            if (updated.Count == 0)
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists);
            }

            var result = new RepositoryResult();
            foreach (var entity in updated)
            {
                result.SucceedIds.Add(entity.Id);
                entity.UpdateFields(entities.First(e => e.Id == entity.Id));
                _context.Update(entity);
            }

            await _context.SaveChangesAsync();
            return result;
        }
    }
}
