using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Interfaces.Repositories;

namespace WorkyOne.Contracts.Requests.Schedule.Common
{
    /// <summary>
    /// Запрос на получение из базы данных DailyInfoEntity
    /// </summary>
    public sealed class DailyInfoRequest : IEntityRequest
    {
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор расписания, для которого требуется получить список DailyInfoEntity
        /// </summary>
        public string ScheduleId { get; set; }
    }
}
