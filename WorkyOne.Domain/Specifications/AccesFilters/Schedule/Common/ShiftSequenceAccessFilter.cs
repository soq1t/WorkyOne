using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common
{
    public sealed class ShiftSequenceAccessFilter : AccessFilter<ShiftSequenceEntity>
    {
        public ShiftSequenceAccessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<ShiftSequenceEntity, bool>> ToExpression()
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
