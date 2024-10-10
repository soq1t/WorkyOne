using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Schedule.Common;

namespace WorkyOne.Repositories.Repositories.Users
{
    /// <summary>
    /// Репозиторий пользовательских данных
    /// </summary>
    public class UserDatasRepository
        : EntityRepository<UserDataEntity, UserDataRequest>,
            IUserDatasRepository
    {
        private readonly ISchedulesRepository _schedulesRepository;

        public UserDatasRepository(
            IBaseRepository baseRepo,
            ApplicationDbContext context,
            ISchedulesRepository schedulesRepository
        )
            : base(baseRepo, context)
        {
            _schedulesRepository = schedulesRepository;
        }

        public override Task<UserDataEntity?> GetAsync(UserDataRequest request)
        {
            var query = _context.UserDatas.Where(d =>
                d.Id == request.Id || d.UserId == request.UserId
            );

            if (request.IncludeFullSchedulesInfo)
            {
                query
                    .Include(d => d.Schedules)
                    .ThenInclude(s => s.Template)
                    .ThenInclude(t => t.Shifts);
                query
                    .Include(d => d.Schedules)
                    .ThenInclude(s => s.Template)
                    .ThenInclude(t => t.Sequences);

                query.Include(d => d.Schedules).ThenInclude(s => s.DatedShifts);
                query.Include(d => d.Schedules).ThenInclude(s => s.PeriodicShifts);
            }
            else if (request.IncludeShortSchedulesInfo)
            {
                query.Include(d => d.Schedules);
            }

            return query.FirstOrDefaultAsync();
        }

        public override async Task<RepositoryResult> UpdateManyAsync(
            ICollection<UserDataEntity> entities
        )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = new RepositoryResult();

                foreach (var entity in entities)
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

        public override async Task<RepositoryResult> UpdateAsync(UserDataEntity entity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var updated = await _context.UserDatas.FirstOrDefaultAsync(e => e.Id == entity.Id);

                if (updated == null)
                {
                    return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
                }

                updated.UpdateFields(entity);
                await _context.SaveChangesAsync();
                var result = new RepositoryResult(updated.Id);

                var operationResult = await _schedulesRepository.RenewAsync(
                    updated.Schedules,
                    entity.Schedules
                );

                result.AddInfo(operationResult);
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
