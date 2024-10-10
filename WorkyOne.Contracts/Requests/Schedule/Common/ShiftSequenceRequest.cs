using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Interfaces.Repositories;
using WorkyOne.Contracts.Requests.Common;

namespace WorkyOne.Contracts.Requests.Schedule.Common
{
    /// <summary>
    /// Запрос на получение ShiftSequenceEntity из базы данных
    /// </summary>
    public class ShiftSequenceRequest : IEntityRequest
    {
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор шаблона, для которого запрашиваются ShiftSequenceEntities
        /// </summary>
        public string TemplateId { get; set; }
    }
}
