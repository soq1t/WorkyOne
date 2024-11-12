using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.Infrastructure.Utilities
{
    public class ReferenceShiftUtility : IReferenceShiftUtility
    {
        private readonly IAccessFiltersStore _accessFilters;

        private readonly IPersonalShiftRepository _personalShiftsRepository;
        private readonly ISharedShiftsRepository _sharedShiftsRepository;

        public ReferenceShiftUtility(
            IAccessFiltersStore accessFilters,
            IPersonalShiftRepository personalShiftsRepository,
            ISharedShiftsRepository sharedShiftsRepository
        )
        {
            _accessFilters = accessFilters;
            _personalShiftsRepository = personalShiftsRepository;
            _sharedShiftsRepository = sharedShiftsRepository;
        }

        public async Task<ShiftEntity?> GetReferenceShift(
            string id,
            CancellationToken cancellation = default
        )
        {
            var personalShift = await _personalShiftsRepository.GetAsync(
                new EntityRequest<PersonalShiftEntity>(
                    new EntityIdFilter<PersonalShiftEntity>(id).And(
                        _accessFilters.GetFilter<PersonalShiftEntity>()
                    )
                ),
                cancellation
            );

            if (personalShift != null)
            {
                return personalShift;
            }

            return await _sharedShiftsRepository.GetAsync(
                new EntityRequest<SharedShiftEntity>(new EntityIdFilter<SharedShiftEntity>(id)),
                cancellation
            );
        }
    }
}
