using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Interfaces;
using WorkyOne.Contracts.Requests.Common;

namespace WorkyOne.Contracts.Requests.Schedule.Shifts
{
    /// <summary>
    /// Запрос на получение ShiftSequenceEntity из базы данных
    /// </summary>
    public class ShiftSequenceRequest : IEntityRequest
    {
        public string Id { get; set; }
        public string TemplateId { get; set; }
    }
}
