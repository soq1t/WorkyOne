using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.AppServices.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория базы для работы с <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Тип данных, с которыми работает репозиторий</typeparam>
    /// <typeparam name="TRequest">Тип запроса на получение данных из базы</typeparam>
    public interface IEntityRepository<TEntity, TRequest>
        where TEntity : class
        where TRequest : class
    {
        public Task<TEntity?> GetAsync(TRequest request);

        public Task<List<TEntity>?> GetManyAsync(TRequest request);

        public Task CreateAsync(TEntity entity);

        public Task UpdateAsync(TEntity entity);

        public Task DeleteAsync(string entityId);
        public Task DeleteManyAsync(List<string> entityIds);
    }
}
