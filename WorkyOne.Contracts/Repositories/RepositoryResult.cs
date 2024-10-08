using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Enums.Reposistories;

namespace WorkyOne.Contracts.Repositories
{
    /// <summary>
    /// Объект, возвращаемый по результату действий обновления, создания и удаления из базы данных репозиторием
    /// </summary>
    public class RepositoryResult
    {
        /// <summary>
        /// Показывает, успешно ли выполнилось действие
        /// </summary>
        public bool IsSuccess => SucceedIds.Any();

        /// <summary>
        /// Список ID сущностей, с которыми удалось успешно совершить действие в базе данных
        /// </summary>
        public List<string> SucceedIds { get; set; } = new List<string>();

        /// <summary>
        /// Список ошибок при неуспешном выполнении действий
        /// </summary>
        public List<RepositoryError> Errors { get; set; } = new List<RepositoryError>();

        /// <summary>
        /// Добавляет информацию об ошибках и ID сущностей, с которыми удалось успешно выполнить операцию
        /// </summary>
        /// <param name="repositoryResult"><see cref="RepositoryResult"/>, из которого берётся информация</param>
        public void AddInfo(RepositoryResult repositoryResult)
        {
            SucceedIds.AddRange(repositoryResult.SucceedIds);
            Errors.AddRange(repositoryResult.Errors);
        }

        /// <summary>
        /// Добавляет <see cref="RepositoryError"/> с указанными <paramref name="errorType"/> и <paramref name="entityId"/> в список ошибок
        /// </summary>
        /// <param name="errorType">Тип ошибки</param>
        /// <param name="entityId">ID сущности, с которой связана <paramref name="errorType"/></param>
        public void AddError(RepositoryErrorType errorType, string? entityId = null)
        {
            Errors.Add(new RepositoryError(errorType, entityId));
        }

        public RepositoryResult() { }

        public RepositoryResult(string succeedId)
        {
            SucceedIds.Add(succeedId);
        }

        public RepositoryResult(IEnumerable<string> suceedIds)
        {
            SucceedIds.AddRange(suceedIds);
        }

        public RepositoryResult(RepositoryError error)
        {
            Errors.Add(error);
        }

        public RepositoryResult(IEnumerable<RepositoryError> errors)
        {
            Errors = new List<RepositoryError>(errors);
        }

        public RepositoryResult(RepositoryErrorType errorType, string? entityId = null)
        {
            RepositoryError error = new RepositoryError(errorType, entityId);
            Errors.Add(error);
        }
    }
}
