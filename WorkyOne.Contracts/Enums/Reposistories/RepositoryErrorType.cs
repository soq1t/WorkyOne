using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Contracts.Enums.Reposistories
{
    /// <summary>
    /// Енам, описывающий тип ошибки при неуспешном выполненни действия репозиторием
    /// </summary>
    public enum RepositoryErrorType
    {
        /// <summary>
        /// Незивестная ошибка
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Сущность не найдена в базе данных
        /// </summary>
        EntityNotExists = 1,

        /// <summary>
        /// Сущность уже существует в базе данных
        /// </summary>
        EntityAlreadyExists = 2,

        /// <summary>
        /// Выполнение действия отменено
        /// </summary>
        OperationCanceled = 3,

        /// <summary>
        /// Сущность не отслеживается контекстом базы данных
        /// </summary>
        EntityNotTrackedByContext = 4,
    }
}
