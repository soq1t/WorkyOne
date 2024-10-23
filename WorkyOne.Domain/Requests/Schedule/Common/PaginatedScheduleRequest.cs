using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Interfaces.Requests.Schedule;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Schedule.Common
{
    /// <inheritdoc/>
    public sealed class PaginatedScheduleRequest
        : PaginatedRequest<ScheduleEntity>,
            IScheduleRequest
    {
        public PaginatedScheduleRequest(string? userDataId = null)
        {
            UserDataId = userDataId;
        }

        private string? _userDataId;

        /// <summary>
        /// Идентификатор <see cref="UserDataEntity"/>, для которых запрашиваются расписания
        /// </summary>
        public string? UserDataId
        {
            get => _userDataId;
            set
            {
                _userDataId = value;

                if (value == null)
                {
                    Predicate = (x) => true;
                }
                else
                {
                    Predicate = (x) => x.UserDataId == value;
                }
            }
        }
        public bool IncludeTemplate { get; set; }

        public bool IncludeShifts { get; set; }
    }
}
