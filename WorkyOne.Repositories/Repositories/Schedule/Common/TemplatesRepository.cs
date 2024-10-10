using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Utilities;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    /// <summary>
    /// Репозиторий по работе с шаблонами
    /// </summary>
    public sealed class TemplatesRepository : ITemplatesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseRepository _baseRepo;
        private readonly ITemplatedShiftsRepository _templatedShiftsRepo;
        private readonly IShiftSequencesRepository _shiftSequencesRepo;

        public TemplatesRepository(
            ApplicationDbContext context,
            ITemplatedShiftsRepository templatedShiftsRepository,
            IShiftSequencesRepository shiftSequencesRepository,
            IBaseRepository baseRepository
        )
        {
            _context = context;
            _templatedShiftsRepo = templatedShiftsRepository;
            _shiftSequencesRepo = shiftSequencesRepository;
            _baseRepo = baseRepository;
        }

        public Task<RepositoryResult> CreateAsync(TemplateEntity entity)
        {
            return _baseRepo.CreateAsync(entity);

            //bool entityExists = await _context.Templates.AnyAsync(t => t.Id == entity.Id);

            //if (!entityExists)
            //{
            //    _context.Templates.Add(entity);
            //    await _context.SaveChangesAsync();
            //    return new RepositoryResult(entity.Id);
            //}
            //else
            //{
            //    return new RepositoryResult(RepositoryErrorType.EntityAlreadyExists, entity.Id);
            //}
        }

        public Task<RepositoryResult> CreateManyAsync(ICollection<TemplateEntity> entities)
        {
            return _baseRepo.CreateManyAsync(entities);

            //List<string> entitiesIds = entities.Select(e => e.Id).ToList();

            //List<string> existedEntitiesIds = await _context
            //    .Templates.Where(t => entitiesIds.Contains(t.Id))
            //    .Select(t => t.Id)
            //    .ToListAsync();

            //if (existedEntitiesIds.Count == entities.Count)
            //{
            //    return new RepositoryResult(RepositoryErrorType.EntityAlreadyExists);
            //}

            //var result = new RepositoryResult();
            //foreach (
            //    TemplateEntity entity in entities.Where(e => !existedEntitiesIds.Contains(e.Id))
            //)
            //{
            //    result.SucceedIds.Add(entity.Id);
            //    _context.Templates.Add(entity);
            //}

            //await _context.SaveChangesAsync();
            //return result;
        }

        public Task<RepositoryResult> DeleteAsync(string entityId)
        {
            return _baseRepo.DeleteAsync<TemplateEntity>(entityId);
            //TemplateEntity? deletedEntity = await _context.Templates.FirstOrDefaultAsync(t =>
            //    t.Id == entityId
            //);

            //if (deletedEntity != null)
            //{
            //    _context.Remove(deletedEntity);
            //    await _context.SaveChangesAsync();
            //    return new RepositoryResult(entityId);
            //}
            //else
            //{
            //    return new RepositoryResult(RepositoryErrorType.EntityNotExists, entityId);
            //}
        }

        public Task<RepositoryResult> DeleteManyAsync(ICollection<string> entityIds)
        {
            return _baseRepo.DeleteManyAsync<TemplateEntity>(entityIds);
            //List<TemplateEntity> deleted = await _context
            //    .Templates.Where(t => entityIds.Contains(t.Id))
            //    .ToListAsync();

            //if (deleted.Count == 0)
            //{
            //    return new RepositoryResult(RepositoryErrorType.EntityNotExists);
            //}
            //else
            //{
            //    _context.RemoveRange(deleted);
            //    await _context.SaveChangesAsync();
            //    return new RepositoryResult(deleted.Select(e => e.Id));
            //}
        }

        public Task<TemplateEntity?> GetAsync(TemplateRequest request)
        {
            if (!string.IsNullOrEmpty(request.Id))
            {
                IQueryable<TemplateEntity> query = _context.Templates.Where(t =>
                    t.Id == request.Id
                );
                query.Include(t => t.Sequences);
                query.Include(t => t.Shifts);

                return query.FirstOrDefaultAsync();
            }

            if (!string.IsNullOrEmpty(request.ScheduleId))
            {
                IQueryable<TemplateEntity> query = _context.Templates.Where(t =>
                    t.ScheduleId == request.ScheduleId
                );
                query.Include(t => t.Sequences);
                query.Include(t => t.Shifts);

                return query.FirstOrDefaultAsync();
            }

            return Task.FromResult<TemplateEntity?>(null);
        }

        public async Task<RepositoryResult> UpdateAsync(TemplateEntity entity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var updated = await GetAsync(
                    new TemplateRequest { Id = entity.Id, ScheduleId = entity.ScheduleId }
                );

                if (updated == null)
                {
                    return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
                }
                var result = new RepositoryResult();
                updated.UpdateFields(entity);
                _context.Update(updated);

                result.SucceedIds.Add(entity.Id);

                var operationResult = await _templatedShiftsRepo.RenewAsync(
                    updated.Shifts,
                    entity.Shifts
                );
                result.AddInfo(operationResult);

                operationResult = await _shiftSequencesRepo.RenewAsync(
                    updated.Sequences,
                    entity.Sequences
                );
                result.AddInfo(operationResult);

                if (result.Errors.Any())
                {
                    await transaction.RollbackAsync();
                    result.SucceedIds.Clear();
                }
                else
                {
                    await transaction.CommitAsync();
                }
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<RepositoryResult> UpdateManyAsync(ICollection<TemplateEntity> entities)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = new RepositoryResult();

                    foreach (TemplateEntity entity in entities)
                    {
                        var operationResult = await UpdateAsync(entity);

                        result.AddInfo(operationResult);
                    }

                    if (result.IsSuccess)
                    {
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

        public Task<RepositoryResult> RenewAsync(
            ICollection<TemplateEntity> oldEntities,
            ICollection<TemplateEntity> newEntities
        ) => DefaultRepositoryMethods.RenewAsync(this, oldEntities, newEntities);
    }
}
