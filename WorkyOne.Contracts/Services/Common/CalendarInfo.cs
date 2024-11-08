namespace WorkyOne.Contracts.Services.Common
{
    /// <summary>
    /// Модель, содержащая информацию о календаре для текущего месяца
    /// </summary>
    public class CalendarInfo
    {
        /// <summary>
        /// Дата, с которой начинается отрисовка календаря
        /// </summary>
        public DateOnly Start { get; set; }

        /// <summary>
        /// Дата, которой оканчивается отрисовка календаря
        /// </summary>
        public DateOnly End { get; set; }
    }
}
