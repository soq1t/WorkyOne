namespace WorkyOne.Contracts.Options.Schedules
{
    /// <summary>
    /// Опции для ролей в рабочем графике
    /// </summary>
    public class RoleOptions
    {
        /// <summary>
        /// На сколько лет вперёд просчитывать график
        /// </summary>
        public int YearsForward { get; set; }

        /// <summary>
        /// На сколько лет назад просчитывать график
        /// </summary>
        public int YearsBackward { get; set; }
    }
}
