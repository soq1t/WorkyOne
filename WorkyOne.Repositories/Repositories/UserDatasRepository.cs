using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Contracts.Requests.Schedule;
using WorkyOne.Domain.Entities;
using WorkyOne.Domain.Entities.Schedule;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Utilities;

namespace WorkyOne.Repositories.Repositories
{
    /// <summary>
    /// Репозиторий пользовательских данных
    /// </summary>
    public class UserDatasRepository : IEntityRepository<UserDataEntity, UserDataRequest>
    {
        private readonly IEntityRepository<UserEntity, UserRequest> _usersRepository;
        private readonly IEntityRepository<ScheduleEntity, ScheduleRequest> _schedulesRepository;
        private readonly ApplicationDbContext _context;

        public UserDatasRepository(
            IEntityRepository<UserEntity, UserRequest> usersRepository,
            ApplicationDbContext context,
            IEntityRepository<ScheduleEntity, ScheduleRequest> schedulesRepository
        )
        {
            _usersRepository = usersRepository;
            _context = context;
            _schedulesRepository = schedulesRepository;
        }

        public async Task<RepositoryResult> CreateAsync(UserDataEntity entity)
        {
            var existed = await _context.UserDatas.AnyAsync(d => d.Id == entity.Id);

            if (existed)
            {
                return new RepositoryResult(RepositoryErrorType.EntityAlreadyExists, entity.Id);
            }

            _context.UserDatas.Add(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult(entity.Id);
        }

        public async Task<RepositoryResult> CreateManyAsync(ICollection<UserDataEntity> entities)
        {
            var ids = entities.Select(e => e.Id).ToList();

            var existedIds = await _context
                .UserDatas.Where(d => ids.Contains(d.Id))
                .Select(d => d.Id)
                .ToListAsync();

            var result = new RepositoryResult();

            foreach (var entity in entities)
            {
                if (existedIds.Contains(entity.Id))
                {
                    result.AddError(RepositoryErrorType.EntityAlreadyExists, entity.Id);
                }
                else
                {
                    result.SucceedIds.Add(entity.Id);
                    _context.UserDatas.Add(entity);
                }
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<RepositoryResult> DeleteAsync(string entityId)
        {
            var deleted = await _context.UserDatas.FirstOrDefaultAsync(e => e.Id == entityId);

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
                .UserDatas.Where(d => entityIds.Contains(d.Id))
                .ToListAsync();

            if (!deleted.Any())
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists);
            }

            var deletedIds = deleted.Select(e => e.Id);
            var notExistedIds = entityIds.Except(deletedIds);

            var result = new RepositoryResult(deletedIds);
            foreach (var item in notExistedIds)
            {
                result.AddError(RepositoryErrorType.EntityNotExists, item);
            }

            foreach (var item in deleted)
            {
                _context.Remove(item);
            }

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<UserDataEntity?> GetAsync(UserDataRequest request)
        {
            var userData = await _context.UserDatas.FirstOrDefaultAsync(d =>
                d.Id == request.Id || d.UserId == request.UserId
            );

            if (userData != null)
            {
                return userData;
            }

            UserRequest userRequest = new UserRequest { Id = request.UserId };
            UserEntity? user = await _usersRepository.GetAsync(userRequest);

            if (user == null)
            {
                return null;
            }

            userData = new UserDataEntity(request.UserId);
            _context.UserDatas.Add(userData);
            await _context.SaveChangesAsync();

            return userData;
        }

        public async Task<ICollection<UserDataEntity>?> GetManyAsync(UserDataRequest request)
        {
            UserDataEntity? userData = await GetAsync(request);
            if (userData == null)
            {
                return null;
            }
            else
            {
                return [userData];
            }
        }

        public Task<RepositoryResult> RenewAsync(
            ICollection<UserDataEntity> oldEntities,
            ICollection<UserDataEntity> newEntities
        )
        {
            return DefaultRepositoryMethods.RenewAsync(this, oldEntities, newEntities);
        }

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
