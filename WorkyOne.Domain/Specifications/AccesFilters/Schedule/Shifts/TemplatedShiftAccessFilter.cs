using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public sealed class TemplatedShiftAccessFilter : AccessFilter<TemplatedShiftEntity>
    {
        public TemplatedShiftAccessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<TemplatedShiftEntity, bool>> ToExpression()
        {
            if (_accessInfo.IsAdmin)
            {
                return x => true;
            }
            else
            {
                return x => x.Template.Schedule.UserDataId == _accessInfo.UserDataId;
            }
        }
    }
}
