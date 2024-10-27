using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Common;

namespace WorkyOne.Contracts.Services.CreateModels.Schedule.Common
{
    /// <summary>
    /// Модель создания последовательности смен для шаблона
    /// </summary>
    public class ShiftSequencesModel
    {
        /// <summary>
        /// Идентификатор шаблона, для которого создаётся последовательность
        /// </summary>
        public string TemplateId { get; set; } = string.Empty;

        /// <summary>
        /// Последовательность смен
        /// </summary>
        [Required]
        public IEnumerable<ShiftSequenceDto> Sequences { get; set; }
    }
}
