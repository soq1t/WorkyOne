using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts.Basic
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class PersonalShiftAcessFilter : AccessFilter<PersonalShiftEntity>
    {
        public PersonalShiftAcessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<PersonalShiftEntity, bool>> ToExpression()
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
