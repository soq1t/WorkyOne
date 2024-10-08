using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
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
    /// Репозиторий пользователей приложения
    /// </summary>
    public class UsersRepository : IEntityRepository<UserEntity, UserRequest>
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly UsersDbContext _context;

        public UsersRepository(UserManager<UserEntity> userManager, UsersDbContext usersDbContext)
        {
            _userManager = userManager;
            _context = usersDbContext;
        }

        public async Task<RepositoryResult> CreateAsync(UserEntity entity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    IdentityResult result = await _userManager.CreateAsync(entity);

                    if (result.Succeeded)
                    {
                        await transaction.CommitAsync();
                        return new RepositoryResult();
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return new RepositoryResult(RepositoryErrorType.Unknown);
                    }
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<RepositoryResult> CreateManyAsync(ICollection<UserEntity> entities)
        {
            using (
                IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync()
            )
            {
                try
                {
                    foreach (UserEntity entity in entities)
                    {
                        RepositoryResult result = await CreateAsync(entity);

                        if (!result.IsSuccess)
                        {
                            await transaction.RollbackAsync();
                            return new RepositoryResult(result);
                        }
                    }

                    await transaction.CommitAsync();
                    return new RepositoryResult();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<RepositoryResult> DeleteAsync(string entityId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    UserEntity? deletedUser = await _userManager.FindByIdAsync(entityId);

                    if (deletedUser == null)
                    {
                        return new RepositoryResult(RepositoryErrorType.EntityNotExists, entityId);
                    }

                    await _userManager.DeleteAsync(deletedUser);
                    await transaction.CommitAsync();
                    return new RepositoryResult();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<RepositoryResult> DeleteManyAsync(ICollection<string> entityIds)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (string entityId in entityIds)
                    {
                        RepositoryResult result = await DeleteAsync(entityId);

                        if (!result.IsSuccess)
                        {
                            await transaction.RollbackAsync();
                            return new RepositoryResult(result);
                        }
                    }

                    await transaction.CommitAsync();
                    return new RepositoryResult();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<UserEntity?> GetAsync(UserRequest request)
        {
            return await _userManager.FindByIdAsync(request.Id);
        }

        public async Task<ICollection<UserEntity>?> GetManyAsync(UserRequest request)
        {
            UserEntity? user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                return null;
            }
            else
            {
                return [user];
            }
        }

        public Task<RepositoryResult> RenewAsync(
            ICollection<UserEntity> oldEntities,
            ICollection<UserEntity> newEntities
        )
        {
            return DefaultRepositoryMethods.RenewAsync(this, oldEntities, newEntities);
        }

        public async Task<RepositoryResult> UpdateAsync(UserEntity entity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    UserEntity? updatedUser = await _userManager.FindByIdAsync(entity.Id);

                    if (updatedUser == null)
                    {
                        return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
                    }

                    updatedUser.UserName = entity.UserName;
                    updatedUser.FirstName = entity.FirstName;
                    updatedUser.IsActivated = entity.IsActivated;

                    await transaction.CommitAsync();
                    return new RepositoryResult();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<RepositoryResult> UpdateManyAsync(ICollection<UserEntity> entities)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var entity in entities)
                    {
                        RepositoryResult result = await UpdateAsync(entity);

                        if (!result.IsSuccess)
                        {
                            await transaction.RollbackAsync();
                            return new RepositoryResult(result);
                        }
                    }

                    await transaction.CommitAsync();
                    return new RepositoryResult();
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
