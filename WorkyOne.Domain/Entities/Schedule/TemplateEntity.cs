using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WorkyOne.Domain.Abstractions;

namespace WorkyOne.Domain.Entities.Schedule
{
    /// <summary>
    /// Сущность, описывающая шаблон для рабочего расписания
    /// </summary>
    public class TemplateEntity : EntityBase
    {
        /// <summary>
        /// Наименование шаблона
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор информации пользователя, к которому относится данный шаблон
        /// </summary>
        [ForeignKey(nameof(UserData))]
        public string UserDataId { get; set; }

        /// <summary>
        /// Информация пользователя, к которому относится данный шаблон
        /// </summary>
        [Required]
        public UserDataEntity UserData { get; set; }

        /// <summary>
        /// Список повторений смен в текущем шаблоне
        /// </summary>
        public List<RepititionEntity> Repititions { get; set; } = new List<RepititionEntity>();

        /// <summary>
        /// Список смен, которые выставляются без повторений
        /// </summary>
        public List<SingleDayShiftEntity> SingleDayShifts { get; set; } =
            new List<SingleDayShiftEntity>();

        /// <summary>
        /// Список смен, используемых в данном шаблоне
        /// </summary>
        public List<ShiftEntity> Shifts { get; set; } = new List<ShiftEntity>();

        /// <summary>
        /// Дата, с которой начинается отсчёт повторений шаблона
        /// </summary>
        [Required]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Указывает, будет ли шаблон рассчитываться в обратную сторону (в прошлое) либо нет
        /// </summary>
        public bool IsMirrored { get; set; } = false;
    }
}
