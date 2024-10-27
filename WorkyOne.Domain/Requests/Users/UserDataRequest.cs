using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Interfaces.Requests.Schedule;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Users
{
    /// <summary>
    /// Запрос на получение <see cref="UserDataEntity"/>
    /// </summary>
    public sealed class UserDataRequest : EntityRequest<UserDataEntity>, IUserDataRequest
    {
        public UserDataRequest(ISpecification<UserDataEntity> specification)
            : base(specification) { }

        public bool IncludeSchedules { get; set; } = false;

        public bool IncludeFullSchedulesInfo { get; set; } = false;
    }
}
