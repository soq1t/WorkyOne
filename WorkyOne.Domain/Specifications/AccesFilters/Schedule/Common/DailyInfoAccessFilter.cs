using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public sealed class DailyInfoAccessFilter : AccessFilter<DailyInfoEntity>
    {
        public DailyInfoAccessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<DailyInfoEntity, bool>> ToExpression()
        {
            if (_accessInfo.IsAdmin)
            {
                return x => true;
            }
            else
            {
                return x => x.Schedule.UserDataId == _accessInfo.UserDataId;
            }
        }
    }
}
