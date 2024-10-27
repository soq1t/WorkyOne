using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Interfaces.Requests.Schedule;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Users
{
    public sealed class PaginatedUserDataRequest
        : PaginatedRequest<UserDataEntity>,
            IUserDataRequest
    {
        public PaginatedUserDataRequest(
            ISpecification<UserDataEntity> specification,
            int pageIndex,
            int amount
        )
            : base(specification, pageIndex, amount) { }

        public bool IncludeSchedules { get; set; } = false;

        public bool IncludeFullSchedulesInfo { get; set; } = false;
    }
}
