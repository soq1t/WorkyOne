using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Entities.Schedule;
using WorkyOne.Domain.Interfaces;

namespace WorkyOne.Domain.Entities
{
    /// <summary>
    /// Сущность, описывающая необходимую приложению информацию о пользователе
    /// </summary>
    public class UserDataEntity : EntityBase, IUpdatable<UserDataEntity>
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

        public void UpdateFields(UserDataEntity entity)
        {
            UserId = entity.UserId;
        }
    }
}
