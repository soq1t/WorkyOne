using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Users
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public sealed class UserDataAccessFilter : AccessFilter<UserDataEntity>
    {
        public UserDataAccessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<UserDataEntity, bool>> ToExpression()
        {
            if (_accessInfo.IsAdmin)
            {
                return x => true;
            }
            else
            {
                return x => x.Id == _accessInfo.UserDataId;
            }
        }
    }
}
