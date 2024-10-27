using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Interfaces.Requests.Schedule;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Schedule.Common
{
    /// <inheritdoc/>
    public sealed class ScheduleRequest : EntityRequest<ScheduleEntity>, IScheduleRequest
    {
        public ScheduleRequest(ISpecification<ScheduleEntity> specification)
            : base(specification) { }

        public bool IncludeTemplate { get; set; }

        public bool IncludeShifts { get; set; }
    }
}
