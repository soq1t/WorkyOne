using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Enums.Reposistories;

namespace WorkyOne.Contracts.Repositories
{
    /// <summary>
    /// Объект, описывающий ошибку при неуспешном выполнении действия репозиторием
    /// </summary>
    public class RepositoryError
    {
        /// <summary>
        /// ID сущности, с которой связана ошибка
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// Тип ошибки
        /// </summary>
        public RepositoryErrorType ErrorType { get; set; }

        /// <summary>
        /// Возвращает сообщение об ошибке
        /// </summary>
        /// <returns></returns>
        public string ErrorMessage()
        {
            StringBuilder result = new StringBuilder();

            if (EntityId != null)
            {
                result.Append($"Сущность [{EntityId}] - ");
            }

            switch (ErrorType)
            {
                case RepositoryErrorType.Unknown:
                    result.Append("Неизвестная ошибка");
                    break;
                case RepositoryErrorType.EntityNotExists:
                    if (EntityId != null)
                        result.Append("Сущность не существует в базе данных");
                    else
                        result.Append("Сущности не существуют в базе данных");
                    break;
                case RepositoryErrorType.EntityAlreadyExists:
                    if (EntityId != null)
                        result.Append("Сущность уже существует в базе данных");
                    else
                        result.Append("Сущности уже существуют в базе данных");
                    break;
            }

            return result.ToString();
        }

        public RepositoryError(RepositoryErrorType type, string? entityId = null)
        {
            ErrorType = type;
            EntityId = entityId;
        }
    }
}
