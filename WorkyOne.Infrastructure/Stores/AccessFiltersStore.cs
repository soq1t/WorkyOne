using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.AccesFilters.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts.Basic;
using WorkyOne.Domain.Specifications.AccesFilters.Users;
using WorkyOne.Infrastructure.Exceptions.Stores;

namespace WorkyOne.Infrastructure.Stores
{
    /// <summary>
    /// Хранилище фильтов доступа к данным
    /// </summary>
    public class AccessFiltersStore : IAccessFiltersStore
    {
        private readonly IUserAccessInfoProvider _accessInfoProvider;

        private readonly Dictionary<Type, object> _filters = [];

        public AccessFiltersStore(IUserAccessInfoProvider accessInfoProvider)
        {
            _accessInfoProvider = accessInfoProvider;
        }

        public async Task CreateFiltersAsync(CancellationToken cancellation = default)
        {
            var accessInfo = await _accessInfoProvider.GetCurrentAsync(cancellation);

            CreateUserFilters(accessInfo);
            CreateScheduleFilters(accessInfo);
        }

        public AccessFilter<T> GetFilter<T>()
            where T : class, IEntity
        {
            _filters.TryGetValue(typeof(T), out var filter);

            if (filter != null)
            {
                return (AccessFilter<T>)filter;
            }
            else
            {
                throw new AccessFilterNotExistsException(typeof(T));
            }
        }

        private void CreateShiftsFilters(UserAccessInfo accessInfo)
        {
            _filters.Add(typeof(PersonalShiftEntity), new PersonalShiftAcessFilter(accessInfo));
            _filters.Add(typeof(SharedShiftEntity), new SharedShiftAccessFilter(accessInfo));

            _filters.Add(typeof(DatedShiftEntity), new DatedShiftAccessFilter(accessInfo));
            _filters.Add(typeof(PeriodicShiftEntity), new PeriodicShiftAccessFilter(accessInfo));
            _filters.Add(typeof(TemplatedShiftEntity), new TemplatedShiftAccessFilter(accessInfo));
        }

        private void CreateScheduleFilters(UserAccessInfo accessInfo)
        {
            _filters.Add(typeof(ScheduleEntity), new ScheduleAccessFilter(accessInfo));
            _filters.Add(typeof(TemplateEntity), new TemplateAccessFilter(accessInfo));

            _filters.Add(typeof(DailyInfoEntity), new DailyInfoAccessFilter(accessInfo));

            CreateShiftsFilters(accessInfo);
        }

        private void CreateUserFilters(UserAccessInfo accessInfo)
        {
            _filters.Add(typeof(UserEntity), new UserAccessFilter(accessInfo));
            _filters.Add(typeof(UserDataEntity), new UserDataAccessFilter(accessInfo));
        }
    }
}
