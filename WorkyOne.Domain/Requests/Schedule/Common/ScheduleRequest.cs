using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Interfaces.Requests.Schedule;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Schedule.Common
{
    /// <inheritdoc/>
    public sealed class ScheduleRequest : EntityRequest<ScheduleEntity>, IScheduleRequest
    {
        public ScheduleRequest(string? scheduleId = null, bool includeFullData = false)
            : base(scheduleId)
        {
            IncludeShifts = includeFullData;
            IncludeTemplate = includeFullData;
        }

        public bool IncludeTemplate { get; set; }

        public bool IncludeShifts { get; set; }
    }
}
