using System.ComponentModel.DataAnnotations;
using WorkyOne.AppServices.DTOs.Abstractions;

namespace WorkyOne.AppServices.DTOs
{
    /// <summary>
    /// DTO шаблона рабочего расписания
    /// </summary>
    public class TemplateDto : DtoBase
    {
        /// <summary>
        /// Идентификатор шаблона
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Наименование шаблона
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Список повторений смен в текущем шаблоне
        /// </summary>
        public List<RepititionDto> Repetitions { get; set; } = new List<RepititionDto>();

        /// <summary>
        /// Список смен, которые выставляются без повторений
        /// </summary>
        public List<SingleDayShiftDto> SingleDayShifts { get; set; } =
            new List<SingleDayShiftDto>();

        /// <summary>
        /// Список смен, используемых в повторениях
        /// </summary>
        public List<ShiftDto> Shifts { get; set; }

        /// <summary>
        /// Дата, с которой начинается отсчёт повторений шаблона
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Указывает, будет ли шаблон рассчитываться в обратную сторону (в прошлое) либо нет
        /// </summary>
        public bool IsMirrored { get; set; }
    }
}
