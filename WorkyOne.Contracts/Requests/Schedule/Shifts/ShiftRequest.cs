using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Requests.Common;

namespace WorkyOne.Contracts.Requests.Schedule.Shifts
{
    /// <summary>
    /// Абстрактный запрос на получение информации о смене
    /// </summary>
    public abstract class ShiftRequest : RequestBase
    {
        public string ShiftId { get; set; }
    }
}
