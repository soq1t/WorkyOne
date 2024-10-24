﻿using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Common
{
    /// <summary>
    /// Интерфейс сервиса работы с рабочим графиком
    /// </summary>
    public interface IWorkGraphicService
    {
        /// <summary>
        /// Возвращает рабочий график для указанного расписания
        /// </summary>
        /// <param name="request">Запрос на получение графика</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<List<DailyInfoDto>> GetGraphicAsync(
            PaginatedWorkGraphicRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт рабочий график согласно заданным в запросе условиям
        /// </summary>
        /// <param name="model">Модель графика</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <returns></returns>
        public Task<ServiceResult> CreateAsync(
            WorkGraphicModel model,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Очищает рабочий график для заданного расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <returns></returns>
        public Task<ServiceResult> ClearAsync(
            string scheduleId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Очищает рабочий график согласно заданным условиям
        /// </summary>
        /// <param name="model">Модель графика</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <returns></returns>
        public Task<ServiceResult> ClearRangeAsync(
            WorkGraphicModel model,
            CancellationToken cancellation = default
        );
    }
}
