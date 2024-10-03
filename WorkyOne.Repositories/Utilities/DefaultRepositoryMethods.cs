using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.AppServices.Interfaces.Repositories;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Domain.Abstractions;

namespace WorkyOne.Repositories.Utilities
{
    /// <summary>
    /// Инструмент, предоставляющий стандартные методы для репозиториев
    /// </summary>
    internal static class DefaultRepositoryMethods
    {
        /// <summary>
        /// Стандартная реализация метода RenewAsync для <see cref="IEntityRepository{TEntity, TRequest}"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности, используемой в репозитории</typeparam>
        /// <typeparam name="TRequest">Тип запроса, используемого в репозитории</typeparam>
        /// <param name="repository">Репозиторий для работы с <typeparamref name="TEntity"/></param>
        /// <param name="oldValues">Список <typeparamref name="TEntity"/>, который должен обновляться</param>
        /// <param name="newValues"></param>
        /// <returns></returns>
        public static async Task RenewAsync<TEntity, TRequest>(
            IEntityRepository<TEntity, TRequest> repository,
            ICollection<TEntity> oldValues,
            ICollection<TEntity> newValues
        )
            where TEntity : EntityBase
            where TRequest : RequestBase
        {
            IEnumerable<string> newValuesIds = newValues.Select(n => n.Id);
            IEnumerable<string> oldValuesIds = oldValues.Select(n => n.Id);

            List<TEntity> removing = oldValues.Where(o => !newValuesIds.Contains(o.Id)).ToList();
            List<TEntity> adding = newValues.Where(n => !oldValuesIds.Contains(n.Id)).ToList();
            List<TEntity> updating = newValues.Where(n => oldValuesIds.Contains(n.Id)).ToList();

            await repository.DeleteManyAsync(removing.Select(r => r.Id).ToList());
            await repository.CreateManyAsync(adding);
            await repository.UpdateManyAsync(updating);
        }
    }
}
