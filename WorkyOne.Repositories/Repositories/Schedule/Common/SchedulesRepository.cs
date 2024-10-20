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

        public override Task<ScheduleEntity?> GetAsync(
            ScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            IQueryable<ScheduleEntity> query = _context.Schedules.Where(s => s.Id == request.Id);

            query = QueryBuilder(request, query);

            return query.FirstOrDefaultAsync(cancellation);
        }

        /// <inheritdoc/>
        public async Task<ICollection<ScheduleEntity>> GetByUserAsync(
            ScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            IQueryable<ScheduleEntity> query = _context.Schedules.Where(s =>
                s.UserDataId == request.UserId
            );

            query = QueryBuilder(request, query);

            return await query.ToListAsync(cancellation);
        }

        /// <summary>
        /// Дополняет запрос на основе <paramref name="request"/>
        /// </summary>
        /// <param name="request">Запрос на получение информации из базы</param>
        /// <param name="query">Сущность запроса</param>
        private IQueryable<ScheduleEntity> QueryBuilder(
            ScheduleRequest request,
            IQueryable<ScheduleEntity> query
        )
        {
            var result = query;
            if (request.IncludeTemplate)
            {
                result = result
                    .Include(s => s.Template)
                    .ThenInclude(t => t.Sequences)
                    .Include(s => s.Template)
                    .ThenInclude(t => t.Shifts);
            }

            if (request.IncludeDatedShifts)
            {
                result = result.Include(s => s.DatedShifts);
            }

            if (request.IncludePeriodicShifts)
            {
                result = result.Include(s => s.PeriodicShifts);
            }

            return result;
        }

        /// <inheritdoc/>
        public override async Task<RepositoryResult> UpdateAsync(
            ScheduleEntity entity,
            CancellationToken cancellation = default
        )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var request = new ScheduleRequest
                {
                    Id = entity.Id,
                    IncludeDatedShifts = true,
                    IncludePeriodicShifts = true,
                    IncludeTemplate = true,
                };

                var updated = await GetAsync(request, cancellation);

                if (updated == null)
                {
                    return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
                }
                var result = new RepositoryResult();

                updated.UpdateFields(entity);
                _context.Update(updated);
                await _context.SaveChangesAsync(cancellation);

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }

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

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }

                var operationResult = await _templatesRepo.RenewAsync(
                    oldTemplates,
                    newTemplates,
                    cancellation
                );
                result.AddInfo(operationResult);

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }

                operationResult = await _datedShiftsRepo.RenewAsync(
                    updated.DatedShifts,
                    entity.DatedShifts
                );
                result.AddInfo(operationResult);

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }

                operationResult = await _periodicShiftsRepo.RenewAsync(
                    updated.PeriodicShifts,
                    entity.PeriodicShifts
                );
                result.AddInfo(operationResult);

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }

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

        /// <inheritdoc/>
        public override async Task<RepositoryResult> UpdateManyAsync(
            ICollection<ScheduleEntity> entities,
            CancellationToken cancellation = default
        )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = new RepositoryResult();

                foreach (var entity in entities)
                {
                    if (cancellation.IsCancellationRequested)
                    {
                        await transaction.RollbackAsync();
                        return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                    }

                    var operationResult = await UpdateAsync(entity, cancellation);
                    result.AddInfo(operationResult);
                }

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
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
