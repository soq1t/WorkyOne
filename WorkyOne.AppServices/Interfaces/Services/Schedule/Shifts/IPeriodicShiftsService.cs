using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts
{
    /// <summary>
    /// Интерфейс сервиса по работе с "периодическими" сменами
    /// </summary>
    public interface IPeriodicShiftsService
    {
        /// <summary>
        /// Возвращает смену по её идентификатору
        /// </summary>
        /// <param name="id">Идентификатор возвращаемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<PeriodicShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает список смен для определённого расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="request">Запрос, содержащий информацию о пагинации</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<List<PeriodicShiftDto>> GetByScheduleIdAsync(
            string scheduleId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает множество смен
        /// </summary>
        /// <param name="request">Запрос, содержащий информацию о пагинации</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<List<PeriodicShiftDto>> GetManyAsync(
            PaginatedRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт смену
        /// </summary>
        /// <param name="model">Модель, слдержащая информацию о создаваемой смене</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> CreateAsync(
            ShiftModel<PeriodicShiftDto> model,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет смену
        /// </summary>
        /// <param name="dto">DTO обновляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> UpdateAsync(
            PeriodicShiftDto dto,
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
