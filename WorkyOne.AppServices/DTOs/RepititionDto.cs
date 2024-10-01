using WorkyOne.AppServices.DTOs.Abstractions;

namespace WorkyOne.AppServices.DTOs
{
    /// <summary>
    /// DTO, описывающее повторение смены
    /// </summary>
    public class RepititionDto : DtoBase
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор смены
        /// </summary>
        public string ShiftId { get; set; }

        /// <summary>
        /// Количество повторений смены
        /// </summary>
        public int RepetitionAmount { get; set; }

        /// <summary>
        /// Порядковый номер повторения в шаблоне
        /// </summary>
        public int Position { get; set; }
    }
}
