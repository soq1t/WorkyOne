using Microsoft.AspNetCore.Identity;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Repositories.Contextes;

namespace WorkyOne.Repositories.Repositories.Users
{
    /// <summary>
    /// Репозиторий пользователей приложения
    /// </summary>
    public class UsersRepository : IUsersRepository
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
                        return new RepositoryResult(entity.Id);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return new RepositoryResult(RepositoryErrorType.Unknown, entity.Id);
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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = new RepositoryResult();
                    foreach (UserEntity entity in entities)
                    {
                        RepositoryResult operationResult = await CreateAsync(entity);

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

                    var operationResult = await _userManager.DeleteAsync(deletedUser);

                    if (operationResult.Succeeded)
                    {
                        await transaction.CommitAsync();
                        return new RepositoryResult(entityId);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return new RepositoryResult(RepositoryErrorType.Unknown, entityId);
                    }
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
                    var result = new RepositoryResult();

                    foreach (string entityId in entityIds)
                    {
                        RepositoryResult operationResult = await DeleteAsync(entityId);

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

        public async Task<RepositoryResult> RenewAsync(
            ICollection<UserEntity> oldEntities,
            ICollection<UserEntity> newEntities
        )
        {
            IEnumerable<string> newValuesIds = newEntities.Select(n => n.Id);
            IEnumerable<string> oldValuesIds = oldEntities.Select(n => n.Id);

            List<UserEntity> removing = oldEntities
                .Where(o => !newValuesIds.Contains(o.Id))
                .ToList();
            List<UserEntity> adding = newEntities.Where(n => !oldValuesIds.Contains(n.Id)).ToList();
            List<UserEntity> updating = newEntities
                .Where(n => oldValuesIds.Contains(n.Id))
                .ToList();

            var result = new RepositoryResult();

            var operationResult = await DeleteManyAsync(removing.Select(r => r.Id).ToList());

            result.AddInfo(operationResult);

            operationResult = await CreateManyAsync(adding);
            result.AddInfo(operationResult);

            operationResult = await UpdateManyAsync(updating);
            result.AddInfo(operationResult);

            return result;
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

                    updatedUser.UpdateFields(entity);
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

        public async Task<RepositoryResult> UpdateManyAsync(ICollection<UserEntity> entities)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = new RepositoryResult();
                    foreach (var entity in entities)
                    {
                        RepositoryResult operationResult = await UpdateAsync(entity);

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
    }
}
