﻿using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Schedule;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Utilities;

namespace WorkyOne.Repositories.Repositories.Schedule
{
    /// <summary>
    /// Репозиторий по работе с шаблонами
    /// </summary>
    public sealed class TemplatesRepository : IEntityRepository<TemplateEntity, TemplateRequest>
    {
        private readonly ApplicationDbContext _context;
        private readonly IEntityRepository<
            TemplatedShiftEntity,
            TemplatedShiftRequest
        > _templatedShiftsRepository;
        private readonly IEntityRepository<
            ShiftSequenceEntity,
            ShiftSequenceRequest
        > _shiftSequencesRepository;

        public TemplatesRepository(
            ApplicationDbContext context,
            IEntityRepository<
                TemplatedShiftEntity,
                TemplatedShiftRequest
            > templatedShiftsRepository,
            IEntityRepository<ShiftSequenceEntity, ShiftSequenceRequest> shiftSequencesRepository
        )
        {
            _context = context;
            _templatedShiftsRepository = templatedShiftsRepository;
            _shiftSequencesRepository = shiftSequencesRepository;
        }

        public async Task<RepositoryResult> CreateAsync(TemplateEntity entity)
        {
            bool entityExists = await _context.Templates.AnyAsync(t => t.Id == entity.Id);

            if (!entityExists)
            {
                _context.Templates.Add(entity);
                await _context.SaveChangesAsync();
                return new RepositoryResult(entity.Id);
            }
            else
            {
                return new RepositoryResult(RepositoryErrorType.EntityAlreadyExists, entity.Id);
            }
        }

        public async Task<RepositoryResult> CreateManyAsync(ICollection<TemplateEntity> entities)
        {
            List<string> entitiesIds = entities.Select(e => e.Id).ToList();

            List<string> existedEntitiesIds = await _context
                .Templates.Where(t => entitiesIds.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync();

            if (existedEntitiesIds.Count == entities.Count)
            {
                return new RepositoryResult(RepositoryErrorType.EntityAlreadyExists);
            }

            var result = new RepositoryResult();
            foreach (
                TemplateEntity entity in entities.Where(e => !existedEntitiesIds.Contains(e.Id))
            )
            {
                result.SucceedIds.Add(entity.Id);
                _context.Templates.Add(entity);
            }

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<RepositoryResult> DeleteAsync(string entityId)
        {
            TemplateEntity? deletedEntity = await _context.Templates.FirstOrDefaultAsync(t =>
                t.Id == entityId
            );

            if (deletedEntity != null)
            {
                _context.Remove(deletedEntity);
                await _context.SaveChangesAsync();
                return new RepositoryResult(entityId);
            }
            else
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists, entityId);
            }
        }

        public async Task<RepositoryResult> DeleteManyAsync(ICollection<string> entityIds)
        {
            List<TemplateEntity> deleted = await _context
                .Templates.Where(t => entityIds.Contains(t.Id))
                .ToListAsync();

            if (deleted.Count == 0)
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists);
            }
            else
            {
                _context.RemoveRange(deleted);
                await _context.SaveChangesAsync();
                return new RepositoryResult(deleted.Select(e => e.Id));
            }
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

        public async Task<ICollection<TemplateEntity>?> GetManyAsync(TemplateRequest request)
        {
            TemplateEntity? template = await GetAsync(request);

            List<TemplateEntity> result = new List<TemplateEntity>();
            if (template != null)
            {
                result.Add(template);
            }
            return result;
        }

        public async Task<RepositoryResult> UpdateAsync(TemplateEntity entity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    TemplateEntity? updatedEntity = await GetAsync(
                        new TemplateRequest { Id = entity.Id, ScheduleId = entity.ScheduleId }
                    );

                    if (updatedEntity == null)
                    {
                        return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
                    }

                    updatedEntity.UpdateFields(entity);
                    _context.Update(updatedEntity);

                    await _templatedShiftsRepository.RenewAsync(
                        updatedEntity.Shifts,
                        entity.Shifts
                    );

                    await _shiftSequencesRepository.RenewAsync(
                        updatedEntity.Sequences,
                        entity.Sequences
                    );

                    await transaction.CommitAsync();
                    return new RepositoryResult(entity.Id);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
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

                        if (operationResult.IsSuccess)
                        {
                            result.SucceedIds.AddRange(operationResult.SucceedIds);
                        }
                    }

                    if (result.SucceedIds.Count == 0)
                    {
                        result.IsSuccess = false;
                        await transaction.RollbackAsync();
                        return result;
                    }

                    await transaction.CommitAsync();
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
