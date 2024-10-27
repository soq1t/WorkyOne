using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public sealed class ScheduleAccessFilter : AccessFilter<ScheduleEntity>
    {
        public ScheduleAccessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<ScheduleEntity, bool>> ToExpression()
        {
            if (_accessInfo.IsAdmin)
            {
                return x => true;
            }
            else
            {
                return x => x.UserDataId == _accessInfo.UserDataId;
            }
        }
    }
}
