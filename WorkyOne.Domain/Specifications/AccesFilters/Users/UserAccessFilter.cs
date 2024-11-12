using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Users
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class UserAccessFilter : AccessFilter<UserEntity>
    {
        public UserAccessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<UserEntity, bool>> ToExpression()
        {
            if (_accessInfo.IsAdmin)
            {
                return x => true;
            }
            else
            {
                return x => x.Id == _accessInfo.UserId;
            }
        }
    }
}
