using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Interfaces;

namespace WorkyOne.Contracts.Requests.Schedule
{
    public class ScheduleRequest : IEntityRequest
    {
        public string Id { get; set; }
    }
}
