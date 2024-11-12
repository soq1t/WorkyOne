using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public sealed class DatedShiftAccessFilter : AccessFilter<DatedShiftEntity>
    {
        public DatedShiftAccessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<DatedShiftEntity, bool>> ToExpression()
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
