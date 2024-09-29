using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Interfaces;

namespace WorkyOne.Domain.Entities.Schedule
{
    /// <summary>
    /// Сущность, описывающая шаблон для рабочего расписания
    /// </summary>
    public class TemplateEntity : EntityBase, IUpdatable<TemplateEntity>
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

        /// <summary>
        /// Конструктор без параметров для EF
        /// </summary>
        public TemplateEntity() { }

        /// <summary>
        /// Конструктор сущности шаблона расписания
        /// </summary>
        /// <param name="name">Название шаблона</param>
        /// <param name="userData">Информация о пользователе, к которому относится шаблон</param>
        public TemplateEntity(string name, UserDataEntity userData, DateOnly startDate)
        {
            Name = name;
            UserData = userData;
            UserDataId = userData.Id;
            StartDate = startDate;
        }

        public void Update(TemplateEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.UserData == null)
            {
                throw new ArgumentNullException(nameof(entity.UserData));
            }

            if (string.IsNullOrEmpty(entity.Name))
            {
                throw new ArgumentException("Название шаблона не может быть пустым!");
            }

            Name = entity.Name;

            Repititions = entity.Repititions ?? new List<RepititionEntity>();
            SingleDayShifts = entity.SingleDayShifts ?? new List<SingleDayShiftEntity>();
            Shifts = entity.Shifts ?? new List<ShiftEntity>();
            StartDate = entity.StartDate;
            IsMirrored = entity.IsMirrored;
        }
    }
}
