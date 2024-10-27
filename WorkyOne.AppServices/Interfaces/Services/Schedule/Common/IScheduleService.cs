using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.GetRequests.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Common
{
    /// <summary>
    /// Интерфейс сервиса по работе с расписанием
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// Возвращает расписание из базы данных
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<ScheduleDto?> GetAsync(
            string scheduleId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает из базы данных расписания, относящиеся к определённому пользователю
        /// </summary>
        /// <param name="userDataId">Идентификатор пользовательских данных</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<List<ScheduleDto>> GetByUserDataAsync(
            string userDataId,
            PaginatedScheduleRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает множестно расписаний из базы данных
        /// </summary>
        /// <param name="request">Запрос на получение данных</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<List<ScheduleDto>> GetManyAsync(
            PaginatedScheduleRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт расписание в базе данных на основании <see cref="ScheduleDto"/>
        /// </summary>
        /// <param name="dto">DTO, на основании которой создаётся расписание</param>
        /// <param name="cancellation">Токен отмены задания</param>

        public Task<ServiceResult> CreateScheduleAsync(
            ScheduleDto dto,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет расписание в базе данных
        /// </summary>
        /// <param name="scheduleDto">DTO обновляемого расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>

        public Task<ServiceResult> UpdateScheduleAsync(
            ScheduleDto scheduleDto,
            CancellationToken cancellation = default
        );
    }
}
