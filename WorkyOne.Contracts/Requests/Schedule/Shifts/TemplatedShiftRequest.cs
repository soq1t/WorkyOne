using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Contracts.Requests.Schedule.Shifts
{
    /// <summary>
    /// Запрос на получение информации о "шаблонной" смене
    /// </summary>
    public sealed class TemplatedShiftRequest : ShiftRequest
    {
        public string TemplateId { get; set; }
    }
}
