using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Common;

namespace WorkyOne.Contracts.Services.CreateModels.Schedule.Common
{
    /// <summary>
    /// Модель, содержащая информацию о создаваемом шаблоне
    /// </summary>
    public class TemplateModel
    {
        /// <summary>
        /// Идентификатор расписания, для которого создаётся шаблон
        /// </summary>
        public string? ScheduleId { get; set; }

        /// <summary>
        /// DTO создаваемого шаблона
        /// </summary>
        [Required]
        public TemplateDto Template { get; set; }
    }
}
