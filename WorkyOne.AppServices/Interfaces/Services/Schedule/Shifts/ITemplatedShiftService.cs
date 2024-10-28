using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts
{
    /// <summary>
    /// Интерфейс сервиса по работе с "шаблонными" сменамиы
    /// </summary>
    public interface ITemplatedShiftService
    {
        /// <summary>
        /// Возвращает <see cref="TemplatedShiftDto"/> из базы данных
        /// </summary>
        /// <param name="id">Идентификатор "шаблонной" смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<TemplatedShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает множество <see cref="TemplatedShiftDto"/> из базы данных
        /// </summary>
        /// <param name="request">Запрос на получение множества <see cref="TemplatedShiftDto"/></param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<List<TemplatedShiftDto>> GetManyAsync(
            PaginatedRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает множество <see cref="TemplatedShiftDto"/>, относящихся к указанному шаблону
        /// </summary>
        /// <param name="templateId">Идентификатор шаблона</param>
        /// <param name="request">Запрос на получение множества <see cref="TemplatedShiftDto"/></param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<List<TemplatedShiftDto>> GetByTemplateIdAsync(
            string templateId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает множество <see cref="TemplatedShiftDto"/>, относящихся к указанному расписанию
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="request">Запрос на получение множества <see cref="TemplatedShiftDto"/></param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<List<TemplatedShiftDto>> GetByScheduleIdAsync(
            string scheduleId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт "шаблонную" смену
        /// </summary>
        /// <param name="model">Модель, содержащая информацию о создаваемой смене</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> CreateAsync(
            ShiftModel<TemplatedShiftDto> model,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет "шаблонную" смену согласно <see cref="TemplatedShiftDto"/>
        /// </summary>
        /// <param name="dto">DTO, согласно которой обновляется смена</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> UpdateAsync(
            TemplatedShiftDto dto,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет шаблонную смену
        /// </summary>
        /// <param name="id">Идентификатор удаляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        );
    }
}
