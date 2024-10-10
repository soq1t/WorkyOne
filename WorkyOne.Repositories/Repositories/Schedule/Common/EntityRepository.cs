using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Interfaces.Repositories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions.Repositories;

namespace WorkyOne.Repositories.Repositories.Schedule.Common
{
    /// <summary>
    /// Абстрактный репозиторий для работы с <typeparamref name="TEntity"/> в базе данных
    /// </summary>
    /// <typeparam name="TEntity">Тип данных, с которыми работает репозиторий</typeparam>
    /// <typeparam name="TRequest">Тип запроса на получение данных из базы</typeparam>
    public abstract class EntityRepository<TEntity, TRequest> : IEntityRepository<TEntity, TRequest>
        where TEntity : EntityBase, IUpdatable<TEntity>
        where TRequest : IEntityRequest
    {
        private readonly IBaseRepository _baseRepo;
        private readonly ApplicationDbContext _context;

        public EntityRepository(IBaseRepository baseRepo, ApplicationDbContext context)
        {
            _baseRepo = baseRepo;
            _context = context;
        }

        public Task<RepositoryResult> CreateAsync(TEntity entity)
        {
            return _baseRepo.CreateAsync(entity);
        }

        public Task<RepositoryResult> CreateManyAsync(ICollection<TEntity> entities)
        {
            return _baseRepo.CreateManyAsync(entities);
        }

        public Task<RepositoryResult> DeleteAsync(string entityId)
        {
            return _baseRepo.DeleteAsync<TEntity>(entityId);
        }

        public Task<RepositoryResult> DeleteManyAsync(ICollection<string> entityIds)
        {
            return _baseRepo.DeleteManyAsync<TEntity>(entityIds);
        }

        public Task<TEntity?> GetAsync(TRequest request)
        {
            return _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == request.Id);
        }

        public Task<RepositoryResult> RenewAsync(
            ICollection<TEntity> oldEntities,
            ICollection<TEntity> newEntities
        )
        {
            return this.DefaultRenewAsync(oldEntities, newEntities);
        }

        public virtual Task<RepositoryResult> UpdateAsync(TEntity entity)
        {
            return this.DefaultUpdateAsync(_context, entity);
        }

        public virtual Task<RepositoryResult> UpdateManyAsync(ICollection<TEntity> entities)
        {
            return this.DefaultUpdateManyAsync(_context, entities);
        }
    }
}
