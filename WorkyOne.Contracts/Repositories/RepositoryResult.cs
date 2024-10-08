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
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Список ID сущностей, с которыми удалось успешно совершить действие в базе данных
        /// </summary>
        public List<string> SucceedIds { get; set; } = new List<string>();

        /// <summary>
        /// Список ошибок при неуспешном выполнении действий
        /// </summary>
        public List<RepositoryError> Errors { get; set; } = new List<RepositoryError>();

        public RepositoryResult(bool isSuccess = true)
        {
            IsSuccess = isSuccess;
        }

        public RepositoryResult(string succeedId)
            : this(true)
        {
            SucceedIds.Add(succeedId);
        }

        public RepositoryResult(IEnumerable<string> suceedIds)
            : this(true)
        {
            SucceedIds.AddRange(suceedIds);
        }

        public RepositoryResult(RepositoryError error)
            : this(false)
        {
            Errors.Add(error);
        }

        public RepositoryResult(IEnumerable<RepositoryError> errors)
            : this(false)
        {
            Errors = new List<RepositoryError>(errors);
        }

        public RepositoryResult(RepositoryResult repositoryResult)
        {
            IsSuccess = repositoryResult.IsSuccess;
            Errors = repositoryResult.Errors;
        }

        public RepositoryResult(RepositoryErrorType errorType, string? entityId = null)
            : this(false)
        {
            RepositoryError error = new RepositoryError(errorType, entityId);
            Errors.Add(error);
        }
    }
}
