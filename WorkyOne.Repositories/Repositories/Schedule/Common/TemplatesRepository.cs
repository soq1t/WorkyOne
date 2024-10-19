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
    /// Репозиторий по работе с шаблонами
    /// </summary>
    public sealed class TemplatesRepository
        : EntityRepository<TemplateEntity, TemplateRequest>,
            ITemplatesRepository
    {
        private readonly ITemplatedShiftsRepository _templatedShiftsRepo;
        private readonly IShiftSequencesRepository _shiftSequencesRepo;

        public TemplatesRepository(
            IBaseRepository baseRepo,
            ApplicationDbContext context,
            ITemplatedShiftsRepository templatedShiftsRepo,
            IShiftSequencesRepository shiftSequencesRepo
        )
            : base(baseRepo, context)
        {
            _templatedShiftsRepo = templatedShiftsRepo;
            _shiftSequencesRepo = shiftSequencesRepo;
        }

        public override Task<TemplateEntity?> GetAsync(
            TemplateRequest request,
            CancellationToken cancellation = default
        )
        {
            if (!string.IsNullOrEmpty(request.Id))
            {
                IQueryable<TemplateEntity> query = _context.Templates.Where(t =>
                    t.Id == request.Id
                );
                query.Include(t => t.Sequences);
                query.Include(t => t.Shifts);

                return query.FirstOrDefaultAsync(cancellation);
            }

            if (!string.IsNullOrEmpty(request.ScheduleId))
            {
                IQueryable<TemplateEntity> query = _context.Templates.Where(t =>
                    t.ScheduleId == request.ScheduleId
                );
                query.Include(t => t.Sequences);
                query.Include(t => t.Shifts);

                return query.FirstOrDefaultAsync(cancellation);
            }

            return Task.FromResult<TemplateEntity?>(null);
        }

        public override async Task<RepositoryResult> UpdateAsync(
            TemplateEntity entity,
            CancellationToken cancellation = default
        )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var updated = await GetAsync(
                    new TemplateRequest { Id = entity.Id, ScheduleId = entity.ScheduleId },
                    cancellation
                );

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

                var operationResult = await _templatedShiftsRepo.RenewAsync(
                    updated.Shifts,
                    entity.Shifts,
                    cancellation
                );
                result.AddInfo(operationResult);

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }

                operationResult = await _shiftSequencesRepo.RenewAsync(
                    updated.Sequences,
                    entity.Sequences,
                    cancellation
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

        public override async Task<RepositoryResult> UpdateManyAsync(
            ICollection<TemplateEntity> entities,
            CancellationToken cancellation = default
        )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = new RepositoryResult();

                foreach (TemplateEntity entity in entities)
                {
                    if (cancellation.IsCancellationRequested)
                    {
                        await transaction.RollbackAsync();
                        return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                    }

                    var operationResult = await UpdateAsync(entity);
                    result.AddInfo(operationResult);
                }

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
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
}
