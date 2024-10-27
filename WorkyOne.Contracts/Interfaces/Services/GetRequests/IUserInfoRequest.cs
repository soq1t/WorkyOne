using WorkyOne.Contracts.DTOs.Common;

namespace WorkyOne.Contracts.Interfaces.Services.GetRequests
{
    /// <summary>
    /// Интерфейс запроса на получение <see cref="UserInfoDto"/>
    /// </summary>
    public interface IUserInfoRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Включать ли в запрос информацию о расписаниях
        /// </summary>
        public bool IncludeSchedules { get; set; }

        /// <summary>
        /// Включать ли в запрос информацию о расписаниях вместе со связанными с ними данными
        /// </summary>
        public bool IncludeFullSchedulesInfo { get; set; }
    }
}
