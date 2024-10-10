using WorkyOne.Contracts.Interfaces.Repositories;

namespace WorkyOne.Contracts.Requests.Schedule.Shifts
{
    /// <summary>
    /// Запрос на получение из базы данных о "периодичной" смене
    /// </summary>
    public sealed class PeriodicShiftRequest : IEntityRequest
    {
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор расписания, для которого требуется получить список смен
        /// </summary>
        public string ScheduleId { get; set; }
    }
}
