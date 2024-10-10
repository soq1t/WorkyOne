using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Interfaces.Repositories;

namespace WorkyOne.Contracts.Requests.Common
{
    /// <summary>
    /// Запрос на получение данных пользователя
    /// </summary>
    public class UserRequest : IEntityRequest
    {
        public string Id { get; set; }
    }
}
