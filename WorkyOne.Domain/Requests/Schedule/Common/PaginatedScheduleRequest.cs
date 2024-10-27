using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Interfaces.Requests.Schedule;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Schedule.Common
{
    /// <inheritdoc/>
    public sealed class PaginatedScheduleRequest
        : PaginatedRequest<ScheduleEntity>,
            IScheduleRequest
    {
        public PaginatedScheduleRequest(
            ISpecification<ScheduleEntity> specification,
            int pageIndex,
            int amount
        )
            : base(specification, pageIndex, amount) { }

        public bool IncludeTemplate { get; set; }

        public bool IncludeShifts { get; set; }
    }
}
