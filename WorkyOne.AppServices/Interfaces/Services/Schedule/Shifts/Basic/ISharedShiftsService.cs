using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic
{
    /// <summary>
    /// Интерфейс сервиса по работе с "общими" сменами
    /// </summary>
    public interface ISharedShiftsService
    {
        /// <summary>
        /// Возвращает смену
        /// </summary>
        /// <param name="id">Идентификатор смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<SharedShiftDto?> GetAsync(string id, CancellationToken cancellation = default);

        /// <summary>
        /// Возвращает список "общих" смен
        /// </summary>
        /// <param name="request">Пагинированный запрос</param>
        /// <param name="cancellation">Токен отмены задачи</param>

        public Task<List<SharedShiftDto>> GetManyAsync(
            PaginatedRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт смену
        /// </summary>
        /// <param name="dto">DTO создаваемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>

        public Task<RepositoryResult> CreateAsync(
            SharedShiftDto dto,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет смену
        /// </summary>
        /// <param name="dto">DTO обновляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>

        public Task<RepositoryResult> UpdateAsync(
            SharedShiftDto dto,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет смену
        /// </summary>
        /// <param name="id">Идентификатор удаляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>

        public Task<RepositoryResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        );
    }
}
