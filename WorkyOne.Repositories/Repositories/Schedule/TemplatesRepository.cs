using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories;
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

        public TemplatesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TemplateEntity entity)
        {
            bool entityExists = await _context.Templates.AnyAsync(t => t.Id == entity.Id);

            if (!entityExists)
            {
                _context.Templates.Add(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateManyAsync(ICollection<TemplateEntity> entities)
        {
            bool changesPerformed = false;

            List<string> entitiesIds = entities.Select(e => e.Id).ToList();

            List<string> existedEntitiesIds = await _context
                .Templates.Where(t => entitiesIds.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync();

            foreach (TemplateEntity entity in entities)
            {
                bool entityExists = existedEntitiesIds.Contains(entity.Id);

                if (!entityExists)
                {
                    _context.Templates.Add(entity);
                    changesPerformed = true;
                }
            }

            if (changesPerformed)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string entityId)
        {
            TemplateEntity? deletedEntity = await _context.Templates.FirstOrDefaultAsync(t =>
                t.Id == entityId
            );

            if (deletedEntity != null)
            {
                _context.Remove(deletedEntity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteManyAsync(ICollection<string> entityIds)
        {
            List<TemplateEntity> deletedEntities = await _context
                .Templates.Where(t => entityIds.Contains(t.Id))
                .ToListAsync();

            if (deletedEntities.Count > 0)
            {
                _context.RemoveRange(deletedEntities);
                await _context.SaveChangesAsync();
            }
        }

        public Task<TemplateEntity?> GetAsync(TemplateRequest request)
        {
            if (!string.IsNullOrEmpty(request.TemplateId))
            {
                IQueryable<TemplateEntity> query = _context.Templates.Where(t =>
                    t.Id == request.TemplateId
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

            return null;
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

        public async Task UpdateAsync(TemplateEntity entity)
        {
            TemplateEntity? updatedEntity = await GetAsync(
                new TemplateRequest { TemplateId = entity.Id }
            );

            if (updatedEntity != null)
            {
                await _templatedShiftsRepository.RenewAsync(updatedEntity.Shifts, entity.Shifts);
                updatedEntity.Shifts = entity.Shifts;

                await _shiftSequencesRepository.RenewAsync(
                    updatedEntity.Sequences,
                    entity.Sequences
                );
                updatedEntity.Sequences = entity.Sequences;
            }
        }

        public async Task UpdateManyAsync(ICollection<TemplateEntity> entities)
        {
            foreach (TemplateEntity entity in entities)
            {
                await UpdateAsync(entity);
            }
        }

        private async Task DeleteShifts(List<string> shiftsIds, TemplateEntity entity)
        {
            List<TemplatedShiftEntity> deletedShifts = entity
                .Shifts.Where(s => !shiftsIds.Contains(s.Id))
                .ToList();

            if (deletedShifts.Count > 0)
            {
                foreach (TemplatedShiftEntity shift in deletedShifts)
                {
                    entity.Shifts.Remove(shift);

                    List<string> deletedShiftsIds = deletedShifts.Select(s => s.Id).ToList();

                    await _templatedShiftsRepository.DeleteManyAsync(deletedShiftsIds);
                }
            }
        }

        private async Task UpdateShifts(List<TemplatedShiftEntity> shifts, TemplateEntity entity)
        {
            List<TemplatedShiftEntity> adding = new List<TemplatedShiftEntity>();
            List<TemplatedShiftEntity> updating = new List<TemplatedShiftEntity>();

            foreach (TemplatedShiftEntity shift in shifts)
            {
                if (entity.Shifts.Any(s => s.Id == shift.Id))
                {
                    updating.Add(shift);
                }
                else
                {
                    adding.Add(shift);
                }
            }

            if (adding.Count > 0)
            {
                entity.Shifts.AddRange(adding);
                await _templatedShiftsRepository.CreateManyAsync(adding);
            }

            if (updating.Count > 0)
            {
                await _templatedShiftsRepository.UpdateManyAsync(updating);

                entity.Shifts = (
                    await _templatedShiftsRepository.GetManyAsync(
                        new TemplatedShiftRequest { TemplateId = entity.Id }
                    )
                ).ToList();
            }
        }

        private async Task UpdateSequences(
            List<ShiftSequenceEntity> sequences,
            TemplateEntity entity
        )
        {
            List<string> deletingIds = entity.Sequences.Select(s => s.Id).ToList();

            await _shiftSequencesRepository.DeleteManyAsync(deletingIds);
            await _shiftSequencesRepository.CreateManyAsync(sequences);

            entity.Sequences = sequences;
        }

        public Task RenewAsync(
            ICollection<TemplateEntity> oldEntities,
            ICollection<TemplateEntity> newEntities
        ) => DefaultRepositoryMethods.RenewAsync(this, oldEntities, newEntities);
    }
}
