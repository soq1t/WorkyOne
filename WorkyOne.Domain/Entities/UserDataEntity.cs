using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.Domain.Entities
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
        /// Список шаблонов рабочих расписаний, которыми управляет пользователь
        /// </summary>
        public List<TemplateEntity> Templates { get; set; } = new List<TemplateEntity>();

        /// <summary>
        /// Конструктор сущности
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, к которому относятся данные</param>
        public UserDataEntity(string userId)
        {
            UserId = userId;
        }
    }
}
