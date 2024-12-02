namespace WorkyOne.Contracts.Options.Schedules
{
    /// <summary>
    /// Опции для рабочего графика
    /// </summary>
    public class WorkGraphicOptions
    {
        /// <summary>
        /// Опции для VIP
        /// </summary>
        public RoleOptions Vip { get; set; }

        /// <summary>
        /// Опции для обычного пользователя
        /// </summary>
        public RoleOptions Default { get; set; }
    }
}
