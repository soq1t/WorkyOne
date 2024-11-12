using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public sealed class PeriodicShiftAccessFilter : AccessFilter<PeriodicShiftEntity>
    {
        public PeriodicShiftAccessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<PeriodicShiftEntity, bool>> ToExpression()
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
