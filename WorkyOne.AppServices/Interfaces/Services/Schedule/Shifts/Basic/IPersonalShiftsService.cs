using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic
{
    /// <summary>
    /// Интерфейс сервиса по работе со сменами в расписании
    /// </summary>
    public interface IPersonalShiftsService
    {
        /// <summary>
        /// Возвращает список смен для определённого расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="request">Пагинированный запрос</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<List<PersonalShiftDto>> GetForScheduleAsync(
            string scheduleId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает смену
        /// </summary>
        /// <param name="id">Идентификатор смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<PersonalShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт смену
        /// </summary>
        /// <param name="model">Модель создаваемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>

        public Task<RepositoryResult> CreateAsync(
            PersonalShiftModel model,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет смену
        /// </summary>
        /// <param name="dto">DTO обновляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>

        public Task<RepositoryResult> UpdateAsync(
            PersonalShiftDto dto,
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
