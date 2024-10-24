using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Schedule.Shifts
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public sealed class PaginatedDatedShiftRequest : PaginatedRequest<DatedShiftEntity>
    {
        /// <summary>
        /// Создаёт запрос на получение множества <see cref="DatedShiftEntity"/> для указанного <see cref="ScheduleEntity"/>
        /// </summary>
        /// <param name="scheduleId">Идентификатор <see cref="ScheduleEntity"/></param>
        public PaginatedDatedShiftRequest(string? scheduleId = null)
        {
            ScheduleId = scheduleId;
        }

        private string? _scheduleId;

        /// <summary>
        /// Идентификатор <see cref="ScheduleEntity"/>, для которого запрашиваются <see cref="DatedShiftEntity"/>
        /// </summary>
        public string? ScheduleId
        {
            get => _scheduleId;
            set
            {
                _scheduleId = value;

                if (value == null)
                {
                    Predicate = (x) => true;
                }
                else
                {
                    Predicate = (x) => x.ScheduleId == value;
                }
            }
        }
    }
}
