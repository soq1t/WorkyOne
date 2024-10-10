using WorkyOne.Contracts.Interfaces.Repositories;

namespace WorkyOne.Contracts.Requests.Schedule.Shifts
{
    /// <summary>
    /// Запрос на получение данных из базы о "датированной" смене
    /// </summary>
    public sealed class DatedShiftRequest : IEntityRequest
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор расписания, для которого требуется получить смены
        /// </summary>
        public string ScheduleId { get; set; }
    }
}
