﻿using System.ComponentModel.DataAnnotations;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Attributes;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Entities.Users
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
        [Renewable]
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
