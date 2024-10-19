using System;
using System.Collections.Generic;
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
        Unknown = 0,
        EntityNotExists = 1,
        EntityAlreadyExists = 2,
        OperationCanceled = 3,
    }
}
