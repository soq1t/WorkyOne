using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Requests.Common;

namespace WorkyOne.Contracts.Requests.Schedule.Shifts
{
    /// <summary>
    /// Запрос на получение ShiftSequenceEntity из базы данных
    /// </summary>
    public class ShiftSequenceRequest : RequestBase
    {
        public string ShiftSequenceId { get; set; }
        public string TemplateId { get; set; }
    }
}
