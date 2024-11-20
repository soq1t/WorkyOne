using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Entities.Abstractions.Common;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Domain.Entities.Users
{
    /// <summary>
    /// Сущность, описывающая необходимую приложению информацию о пользователе
    /// </summary>
    public class UserDataEntity : EntityBase
    {
        /// <summary>
        /// Идентификатор пользователя, к которому относится информация
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Список рабочих графиков, которые создал пользователь
        /// </summary>
        public List<ScheduleEntity> Schedules { get; set; } = new List<ScheduleEntity>();

        /// <summary>
        /// Идентификатор избранного расписания
        /// </summary>
        [ForeignKey(nameof(FavoriteSchedule))]
        [AutoUpdated]
        public string? FavoriteScheduleId { get; set; }

        /// <summary>
        /// Избранное расписание
        /// </summary>
        public ScheduleEntity? FavoriteSchedule { get; set; }

        /// <summary>
        /// Конструктор сущности
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, к которому относятся данные</param>
        public UserDataEntity(string userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Стандартный конструктор сущности (для EF)
        /// </summary>
        public UserDataEntity() { }
    }
}
