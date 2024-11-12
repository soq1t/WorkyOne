using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts.Basic
{
    public class SharedShiftAccessFilter : AccessFilter<SharedShiftEntity>
    {
        public SharedShiftAccessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<SharedShiftEntity, bool>> ToExpression()
        {
            return x => true;
        }
    }
}
