using WorkyOne.Domain.Entities.Abstractions.Shifts;

namespace WorkyOne.AppServices.Interfaces.Utilities
{
    /// <summary>
    /// Интерфейс инструмента, позволяющего получить <see cref="ShiftEntity"/>
    /// </summary>
    public interface IReferenceShiftUtility
    {
        /// <summary>
        /// Возвращает <see cref="ShiftEntity"/> по её идентификатору
        /// </summary>
        /// <param name="id">Идентификатор смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<ShiftEntity?> GetReferenceShift(
            string id,
            CancellationToken cancellation = default
        );
    }
}
