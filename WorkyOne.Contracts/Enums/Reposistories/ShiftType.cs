namespace WorkyOne.Contracts.Enums.Reposistories
{
    /// <summary>
    /// Тип смены
    /// </summary>
    public enum ShiftType
    {
        /// <summary>
        /// Общая для всего приложения
        /// </summary>
        Shared = 0,

        /// <summary>
        /// Используется только в расписании
        /// </summary>
        Schedule = 1
    }
}
