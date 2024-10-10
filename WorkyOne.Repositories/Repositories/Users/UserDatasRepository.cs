using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Utilities;

namespace WorkyOne.Repositories.Repositories.Users
{
    /// <summary>
    /// Репозиторий пользовательских данных
    /// </summary>
    public class UserDatasRepository : IEntityRepository<UserDataEntity, UserDataRequest>
    {
        private readonly IBaseRepository _baseRepo;
        private readonly IEntityRepository<ScheduleEntity, ScheduleRequest> _schedulesRepository;
        private readonly ApplicationDbContext _context;

        public UserDatasRepository(
            ApplicationDbContext context,
            IEntityRepository<ScheduleEntity, ScheduleRequest> schedulesRepository,
            IBaseRepository baseRepository
        )
        {
            _context = context;
            _schedulesRepository = schedulesRepository;
            _baseRepo = baseRepository;
        }

        public Task<RepositoryResult> CreateAsync(UserDataEntity entity)
        {
            return _baseRepo.CreateAsync(entity);

            //var existed = await _context.UserDatas.AnyAsync(d => d.Id == entity.Id);

            //if (existed)
            //{
            //    return new RepositoryResult(RepositoryErrorType.EntityAlreadyExists, entity.Id);
            //}

            //_context.UserDatas.Add(entity);
            //await _context.SaveChangesAsync();
            //return new RepositoryResult(entity.Id);
        }

        public Task<RepositoryResult> CreateManyAsync(ICollection<UserDataEntity> entities)
        {
            return _baseRepo.CreateManyAsync(entities);

            //var ids = entities.Select(e => e.Id).ToList();

            //var existedIds = await _context
            //    .UserDatas.Where(d => ids.Contains(d.Id))
            //    .Select(d => d.Id)
            //    .ToListAsync();

            //var result = new RepositoryResult();

            //foreach (var entity in entities)
            //{
            //    if (existedIds.Contains(entity.Id))
            //    {
            //        result.AddError(RepositoryErrorType.EntityAlreadyExists, entity.Id);
            //    }
            //    else
            //    {
            //        result.SucceedIds.Add(entity.Id);
            //        _context.UserDatas.Add(entity);
            //    }
            //}

            //if (_context.ChangeTracker.HasChanges())
            //{
            //    await _context.SaveChangesAsync();
            //}

            //return result;
        }

        public Task<RepositoryResult> DeleteAsync(string entityId)
        {
            return _baseRepo.DeleteAsync<UserDataEntity>(entityId);

            //var deleted = await _context.UserDatas.FirstOrDefaultAsync(e => e.Id == entityId);

            //if (deleted == null)
            //{
            //    return new RepositoryResult(RepositoryErrorType.EntityNotExists, entityId);
            //}

            //_context.Remove(deleted);
            //await _context.SaveChangesAsync();
            //return new RepositoryResult(entityId);
        }

        public Task<RepositoryResult> DeleteManyAsync(ICollection<string> entityIds)
        {
            return _baseRepo.DeleteManyAsync<UserDataEntity>(entityIds);

            //var deleted = await _context
            //    .UserDatas.Where(d => entityIds.Contains(d.Id))
            //    .ToListAsync();

            //if (!deleted.Any())
            //{
            //    return new RepositoryResult(RepositoryErrorType.EntityNotExists);
            //}

            //var deletedIds = deleted.Select(e => e.Id);
            //var notExistedIds = entityIds.Except(deletedIds);

            //var result = new RepositoryResult(deletedIds);
            //foreach (var item in notExistedIds)
            //{
            //    result.AddError(RepositoryErrorType.EntityNotExists, item);
            //}

            //foreach (var item in deleted)
            //{
            //    _context.Remove(item);
            //}

            //await _context.SaveChangesAsync();
            //return result;
        }

        public async Task<UserDataEntity?> GetAsync(UserDataRequest request)
        {
            return _context.UserDatas.FirstOrDefault(d =>
                d.Id == request.Id || d.UserId == request.UserId
            );

            //var userData = await _context.UserDatas.FirstOrDefaultAsync(d =>
            //    d.Id == request.Id || d.UserId == request.UserId
            //);

            //if (userData != null)
            //{
            //    return userData;
            //}

            //UserRequest userRequest = new UserRequest { Id = request.UserId };
            //UserEntity? user = await _usersRepository.GetAsync(userRequest);

            //if (user == null)
            //{
            //    return null;
            //}

            //userData = new UserDataEntity(request.UserId);
            //_context.UserDatas.Add(userData);
            //await _context.SaveChangesAsync();

            //return userData;
        }

        //public async Task<ICollection<UserDataEntity>?> GetManyAsync(UserDataRequest request)
        //{
        //    UserDataEntity? userData = await GetAsync(request);
        //    if (userData == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return [userData];
        //    }
        //}

        public Task<RepositoryResult> RenewAsync(
            ICollection<UserDataEntity> oldEntities,
            ICollection<UserDataEntity> newEntities
        ) => DefaultRepositoryMethods.RenewAsync(this, oldEntities, newEntities);

        public async Task<RepositoryResult> UpdateManyAsync(ICollection<UserDataEntity> entities)
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

        public async Task<RepositoryResult> UpdateAsync(UserDataEntity entity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var updated = await _context.UserDatas.FirstOrDefaultAsync(e =>
                        e.Id == entity.Id
                    );

                    if (updated == null)
                    {
                        return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
                    }

                    updated.UpdateFields(entity);

                    await _schedulesRepository.RenewAsync(updated.Schedules, entity.Schedules);

                    if (_context.ChangeTracker.HasChanges())
                    {
                        await _context.SaveChangesAsync();
                    }

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
    }
}
