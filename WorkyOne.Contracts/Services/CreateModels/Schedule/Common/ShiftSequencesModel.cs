using WorkyOne.Contracts.DTOs.Schedule.Common;

namespace WorkyOne.Contracts.Services.CreateModels.Schedule.Common
{
    /// <summary>
    /// Модель создания последовательности смен для шаблона
    /// </summary>
    public class ShiftSequencesModel
    {
        /// <summary>
        /// Идентификатор шаблона
        /// </summary>
        public string? TemplateId { get; set; }

        /// <summary>
        /// Последовательность смен
        /// </summary>
        public IEnumerable<ShiftSequenceDto> Sequences { get; set; } = new List<ShiftSequenceDto>();
    }
}
