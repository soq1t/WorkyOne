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

        /// <summary>
        /// Количество дней в промежутке между <see cref="Start"/> и <see cref="End"/>
        /// </summary>
        public int DaysAmount { get; set; }

        /// <summary>
        /// Название месяца, для которого отрисовывается календарь календарь
        /// </summary>
        public string MonthName { get; set; }

        /// <summary>
        /// Номер месяца, для которого отрисовывается календарь календарь
        /// </summary>
        public int MonthNumber { get; set; }

        /// <summary>
        /// Год, для которого отрисовывается календарь календарь
        /// </summary>
        public int Year { get; set; }
    }
}
