using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Utilities;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    /// <summary>
    /// Репозиторий по работе с <see cref="ShiftSequenceEntity"/>
    /// </summary>
    public sealed class ShiftSequencesRepository : IShiftSequencesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseRepository _baseRepo;

        public ShiftSequencesRepository(ApplicationDbContext context, IBaseRepository baseRepo)
        {
            _context = context;
            _baseRepo = baseRepo;
        }

        public Task<RepositoryResult> CreateAsync(ShiftSequenceEntity entity)
        {
            return _baseRepo.CreateAsync(entity);
        }

        public Task<RepositoryResult> CreateManyAsync(ICollection<ShiftSequenceEntity> entities)
        {
            return _baseRepo.CreateManyAsync(entities);
        }

        public Task<RepositoryResult> DeleteAsync(string entityId)
        {
            return _baseRepo.DeleteAsync<ShiftSequenceEntity>(entityId);
        }

        public Task<RepositoryResult> DeleteManyAsync(ICollection<string> entityIds)
        {
            return _baseRepo.DeleteManyAsync<ShiftSequenceEntity>(entityIds);
        }

        public Task<ShiftSequenceEntity?> GetAsync(ShiftSequenceRequest request)
        {
            return _context.ShiftSequences.FirstOrDefaultAsync(s => s.Id == request.Id);
        }

        public async Task<ICollection<ShiftSequenceEntity>> GetByTemplateIdAsync(
            ShiftSequenceRequest request
        )
        {
            return await _context
                .ShiftSequences.Where(s => s.TemplateId == request.TemplateId)
                .ToListAsync();
        }

        public Task<RepositoryResult> RenewAsync(
            ICollection<ShiftSequenceEntity> oldEntities,
            ICollection<ShiftSequenceEntity> newEntities
        )
        {
            return DefaultRepositoryMethods.RenewAsync(this, oldEntities, newEntities);
        }

        public async Task<RepositoryResult> UpdateAsync(ShiftSequenceEntity entity)
        {
            var updated = await _context.ShiftSequences.FirstOrDefaultAsync(s => s.Id == entity.Id);

            if (updated == null)
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
            }

            updated.UpdateFields(entity);
            _context.Update(updated);
            await _context.SaveChangesAsync();
            return new RepositoryResult(updated.Id);
        }

        public async Task<RepositoryResult> UpdateManyAsync(
            ICollection<ShiftSequenceEntity> entities
        )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = new RepositoryResult();

                var ids = entities.Select(e => e.Id).ToList();
                var updated = await _context
                    .ShiftSequences.Where(s => ids.Contains(s.Id))
                    .ToListAsync();
                var updatedIds = updated.Select(s => s.Id).ToList();
                var notExistedIds = ids.Except(updatedIds).ToList();

                notExistedIds.ForEach(id =>
                    result.AddError(RepositoryErrorType.EntityNotExists, id)
                );
                result.SucceedIds.AddRange(updatedIds);

                foreach (var item in updated)
                {
                    item.UpdateFields(entities.First(e => e.Id == item.Id));
                    _context.Update(item);
                }

                if (result.IsSuccess)
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
