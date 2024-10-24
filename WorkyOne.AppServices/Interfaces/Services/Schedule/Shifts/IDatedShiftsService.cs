using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts
{
    /// <summary>
    /// Интерфейс сервиса по работе с <see cref="DatedShiftEntity"/>
    /// </summary>
    public interface IDatedShiftsService
    {
        /// <summary>
        /// Возвращает список <see cref="DatedShiftDto"/> для указанного расписания
        /// </summary>
        /// <param name="request">Запрос на получение данных</param>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<List<DatedShiftDto>> GetForScheduleAsync(
            string scheduleId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает <see cref="DatedShiftDto"/> по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>
        public Task<DatedShiftDto?> GetAsync(string id, CancellationToken cancellation = default);

        /// <summary>
        /// Возвращает множество <see cref="DatedShiftDto"/> согласно запросу
        /// </summary>
        /// <param name="request">Запрос на получение данных</param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>
        public Task<List<DatedShiftDto>> GetManyAsync(
            PaginatedRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет из базы данных <see cref="DatedShiftEntity"/>
        /// </summary>
        /// <param name="id">Идентификатор удаляемого <see cref="DatedShiftEntity"/></param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>

        public Task<ServiceResult> DeleteAsync(string id, CancellationToken cancellation = default);

        /// <summary>
        /// Создаёт в базе данных <see cref="DatedShiftEntity"/> на основе передаваемой <see cref="DatedShiftDto"/>
        /// </summary>
        /// <param name="dto">DTO, на основе которой будет создана <see cref="DatedShiftEntity"/></param>
        /// <param name="scheduleId">Идентификатор <see cref="ScheduleEntity"/>, для которого создаётся <see cref="DatedShiftEntity"/></param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>
        public Task<ServiceResult> CreateAsync(
            DatedShiftDto dto,
            string scheduleId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет в базе данных <see cref="DatedShiftEntity"/> на основании передаваемой <see cref="DatedShiftDto"/> по её идентификатору
        /// </summary>
        /// <param name="dto">DTO, данными которой будет обновлена <see cref="DatedShiftEntity"/> в базе данных</param>
        /// <param name="cancel">Токен отмены задания</param>
        /// <returns></returns>
        public Task<ServiceResult> UpdateAsync(
            DatedShiftDto dto,
            CancellationToken cancel = default
        );
    }
}
