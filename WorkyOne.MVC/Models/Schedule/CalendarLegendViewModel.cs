namespace WorkyOne.MVC.Models.Schedule
{
    /// <summary>
    /// Вью модель для легенды календаря
    /// </summary>
    public class CalendarLegendViewModel
    {
        /// <summary>
        /// Легенда для календаря
        /// </summary>
        public Dictionary<string, string> Legend { get; set; } = [];
    }
}
