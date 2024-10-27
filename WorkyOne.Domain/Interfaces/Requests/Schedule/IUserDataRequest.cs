using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.Domain.Interfaces.Requests.Schedule
{
    /// <summary>
    /// Интерфейс запроса на получение <see cref="UserDataEntity"/>
    /// </summary>
    public interface IUserDataRequest
    {
        /// <summary>
        /// Включать ли в запрашиваемую сущность список расписаний
        /// </summary>
        public bool IncludeSchedules { get; }

        /// <summary>
        /// Включать ли в запрашиваемую сущность полную информацию о расписаниях (вместе со связанными сущностями)
        /// </summary>
        public bool IncludeFullSchedulesInfo { get; }
    }
}
