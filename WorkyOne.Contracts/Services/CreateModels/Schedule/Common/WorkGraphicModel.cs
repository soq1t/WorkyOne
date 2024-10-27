using WorkyOne.Contracts.Attributes.Validation;

namespace WorkyOne.Contracts.Services.CreateModels.Schedule.Common
{
    /// <summary>
    /// Модель, содержащая информацию для создания/удаления рабочего графика
    /// </summary>
    public class WorkGraphicModel
    {
        /// <summary>
        /// Идентификатор расписания
        /// </summary>
        public string? ScheduleId { get; set; }

        /// <summary>
        /// Дата начала рабочего графика
        /// </summary>
        [DateCompare(Enums.Attributes.DateCompareMode.LessOrEquial, nameof(EndDate))]
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// Дата окончания рабочего графика
        /// </summary>
        [DateCompare(Enums.Attributes.DateCompareMode.MoreOrEquial, nameof(StartDate))]
        public DateOnly? EndDate { get; set; }
    }
}
