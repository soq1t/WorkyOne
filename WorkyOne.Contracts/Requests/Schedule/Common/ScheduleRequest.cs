using WorkyOne.Contracts.Interfaces.Repositories;

namespace WorkyOne.Contracts.Requests.Schedule.Common
{
    /// <summary>
    /// Запрос на получение ScheduleEntity из базы данных
    /// </summary>
    public class ScheduleRequest : IEntityRequest
    {
        /// <summary>
        /// Идентификатор получаемой сущности
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор пользовательских данных, для которых запрашиваются ScheduleEntities
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Включать ли "шаблон" в запрашиваемую сущность
        /// </summary>
        public bool IncludeTemplate { get; set; } = false;

        /// <summary>
        /// Включать ли "датированные" смены в запрашиваемую сущность
        /// </summary>
        public bool IncludeDatedShifts { get; set; } = false;

        /// <summary>
        /// Включать ли "периодические" смены в запрашиваемую сущность
        /// </summary>
        public bool IncludePeriodicShifts { get; set; } = false;
    }
}
