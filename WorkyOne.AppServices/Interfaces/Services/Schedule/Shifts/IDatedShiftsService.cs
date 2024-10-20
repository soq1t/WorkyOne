using WorkyOne.Contracts.DTOs.Schedule.Shifts;
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
        /// Возвращает список <see cref="DatedShiftDto"/> для указанного <see cref="ScheduleEntity"/>
        /// </summary>
        /// <param name="scheduleId">Идентификатор <see cref="ScheduleEntity"/></param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>
        public Task<List<DatedShiftDto>> GetForScheduleAsync(
            string scheduleId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает <see cref="DatedShiftDto"/> по идентификатору <see cref="DatedShiftEntity"/>
        /// </summary>
        /// <param name="id">Идентификатор <see cref="DatedShiftEntity"/></param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>
        public Task<DatedShiftDto?> GetAsync(string id, CancellationToken cancellation = default);

        /// <summary>
        /// Удаляет из базы данных множество <see cref="DatedShiftEntity"/>
        /// </summary>
        /// <param name="ids">Список идентификаторов удаляемых <see cref="DatedShiftEntity"/></param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>
        public Task<bool> DeleteManyAsync(
            ICollection<string> ids,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет из базы данных <see cref="DatedShiftEntity"/>
        /// </summary>
        /// <param name="id">Идентификатор удаляемого <see cref="DatedShiftEntity"/></param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>

        public Task<bool> DeleteAsync(string id, CancellationToken cancellation = default);

        /// <summary>
        /// Удаляет все <see cref="DatedShiftEntity"/> для указанного <see cref="ScheduleEntity"/>
        /// </summary>
        /// <param name="scheduleId">Идентификатор <see cref="ScheduleEntity"/></param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>
        public Task<bool> DeleteForScheduleAsync(
            string scheduleId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт в базе данных <see cref="DatedShiftEntity"/> на основе передаваемой <see cref="DatedShiftDto"/>
        /// </summary>
        /// <param name="dto">DTO, на основе которой будет создана <see cref="DatedShiftEntity"/></param>
        /// <param name="scheduleId">Идентификатор <see cref="ScheduleEntity"/>, для которого создаётся <see cref="DatedShiftEntity"/></param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>
        public Task<bool> CreateAsync(
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
        public Task<bool> UpdateAsync(DatedShiftDto dto, CancellationToken cancel = default);
    }
}
