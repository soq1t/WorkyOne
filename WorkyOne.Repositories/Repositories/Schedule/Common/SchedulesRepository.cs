using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Common;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    /// <summary>
    /// Репозиторий по работе с <see cref="ScheduleEntity"/>
    /// </summary>
    public sealed class SchedulesRepository
        : EntityRepository<ScheduleEntity, ScheduleRequest>,
            ISchedulesRepository
    {
        private readonly ITemplatesRepository _templatesRepo;
        private readonly IDatedShiftsRepository _datedShiftsRepo;
        private readonly IPeriodicShiftsRepository _periodicShiftsRepo;

        public SchedulesRepository(
            IBaseRepository baseRepo,
            ApplicationDbContext context,
            ITemplatesRepository templatesRepo,
            IDatedShiftsRepository datedShiftsRepo,
            IPeriodicShiftsRepository periodicShiftsRepo
        )
            : base(baseRepo, context)
        {
            _templatesRepo = templatesRepo;
            _datedShiftsRepo = datedShiftsRepo;
            _periodicShiftsRepo = periodicShiftsRepo;
        }

        public override Task<ScheduleEntity?> GetAsync(ScheduleRequest request)
        {
            var query = _context.Schedules.Where(s => s.Id == request.Id);

            QueryBuilder(request, query);

            return query.FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<ICollection<ScheduleEntity>> GetByUserAsync(ScheduleRequest request)
        {
            var query = _context.Schedules.Where(s => s.UserDataId == request.UserId);

            QueryBuilder(request, query);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Дополняет запрос на основе <paramref name="request"/>
        /// </summary>
        /// <param name="request">Запрос на получение информации из базы</param>
        /// <param name="query">Сущность запроса</param>
        private void QueryBuilder(ScheduleRequest request, IQueryable<ScheduleEntity> query)
        {
            if (request.IncludeTemplate)
            {
                query
                    .Include(s => s.Template)
                    .ThenInclude(t => t.Sequences)
                    .Include(s => s.Template)
                    .ThenInclude(t => t.Shifts);
            }

            if (request.IncludeDatedShifts)
            {
                query.Include(s => s.DatedShifts);
            }

            if (request.IncludePeriodicShifts)
            {
                query.Include(s => s.PeriodicShifts);
            }
        }

        /// <inheritdoc/>
        public override async Task<RepositoryResult> UpdateAsync(ScheduleEntity entity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var request = new ScheduleRequest
                    {
                        Id = entity.Id,
                        IncludeDatedShifts = true,
                        IncludePeriodicShifts = true,
                        IncludeTemplate = true,
                    };
                    var updated = await GetAsync(request);

                    if (updated == null)
                    {
                        return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
                    }
                    var result = new RepositoryResult();

                    updated.UpdateFields(entity);
                    _context.Update(updated);

                    result.SucceedIds.Add(entity.Id);

                    var oldTemplates = new List<TemplateEntity>();

                    if (updated.Template != null)
                    {
                        oldTemplates.Add(updated.Template);
                    }

                    var newTemplates = new List<TemplateEntity>();

                    if (entity.Template != null)
                    {
                        oldTemplates.Add(entity.Template);
                    }

                    var operationResult = await _templatesRepo.RenewAsync(
                        oldTemplates,
                        newTemplates
                    );
                    result.AddInfo(operationResult);

                    operationResult = await _datedShiftsRepo.RenewAsync(
                        updated.DatedShifts,
                        entity.DatedShifts
                    );

                    result.AddInfo(operationResult);

                    operationResult = await _periodicShiftsRepo.RenewAsync(
                        updated.PeriodicShifts,
                        entity.PeriodicShifts
                    );

                    result.AddInfo(operationResult);

                    if (result.Errors.Any())
                    {
                        await transaction.RollbackAsync();
                        result.SucceedIds.Clear();
                        return result;
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

        /// <inheritdoc/>
        public override async Task<RepositoryResult> UpdateManyAsync(
            ICollection<ScheduleEntity> entities
        )
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = new RepositoryResult();

                    foreach (var entity in entities)
                    {
                        var operationResult = await UpdateAsync(entity);
                        result.AddInfo(operationResult);
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
    }
}
