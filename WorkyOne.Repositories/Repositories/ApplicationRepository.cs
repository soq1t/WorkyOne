using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Entities.Schedule;
using WorkyOne.Domain.Interfaces;
using WorkyOne.Repositories.Contextes;

namespace WorkyOne.Repositories.Repositories
{
    /// <summary>
    /// Базовый репозиторий приложения
    /// </summary>
    public class ApplicationRepository : IBaseRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResult> CreateAsync<TEntity>(TEntity entity)
            where TEntity : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResult> CreateManyAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResult> DeleteAsync<TEntity>(string entityId)
            where TEntity : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResult> DeleteManyAsync<TEntity>(IEnumerable<string> entitiesIds)
            where TEntity : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<TEntity?> GetAsync<TEntity>(IQueryable<TEntity> query)
            where TEntity : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetManyAsync<TEntity>(IQueryable<TEntity> query)
            where TEntity : EntityBase
        {
            throw new NotImplementedException();
        }
    }
}
