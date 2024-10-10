using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Interfaces.Repositories;

namespace WorkyOne.Contracts.Requests.Common
{
    /// <summary>
    /// Запрос на получение пользовательских данных из базы данных
    /// </summary>
    public class UserDataRequest : IEntityRequest
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public bool IncludeFullSchedulesInfo { get; set; } = false;
        public bool IncludeShortSchedulesInfo { get; set; } = false;
    }
}
