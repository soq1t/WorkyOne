using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Contracts.Services.Requests
{
    public class MonthGraphicRequest : CalendarInfoRequest
    {
        public string? ScheduleId { get; set; }
    }
}
