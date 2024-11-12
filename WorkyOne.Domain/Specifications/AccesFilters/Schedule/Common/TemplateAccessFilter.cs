using System.Linq.Expressions;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common
{
    public sealed class TemplateAccessFilter : AccessFilter<TemplateEntity>
    {
        public TemplateAccessFilter(UserAccessInfo accessInfo)
            : base(accessInfo) { }

        public override Expression<Func<TemplateEntity, bool>> ToExpression()
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
