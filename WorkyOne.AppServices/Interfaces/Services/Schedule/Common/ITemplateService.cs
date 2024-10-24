﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Common
{
    /// <summary>
    /// Интерфейс сервиса по работе с шаблонами
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Возвращает <see cref="TemplateDto"/> из базы данных
        /// </summary>
        /// <param name="id">Идентификатор шаблона</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<TemplateDto?> GetAsync(string id, CancellationToken cancellation = default);

        /// <summary>
        /// Возвращает <see cref="TemplateDto"/> согласно расписанию, к которому он относится
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<TemplateDto?> GetByScheduleIdAsync(
            string scheduleId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт шаблон на основе <see cref="TemplateDto"/>
        /// </summary>
        /// <param name="dto">DTO создаваемого шаблона</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<ServiceResult> CreateAsync(
            TemplateDto dto,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет шаблон из базы данных
        /// </summary>
        /// <param name="id">Идентификатор шаблона</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<ServiceResult> DeleteAsync(string id, CancellationToken cancellation = default);

        /// <summary>
        /// Обновляет шаблон на основе <see cref="TemplateDto"/>
        /// </summary>
        /// <param name="dto">DTO, на основе которой обновляется шаблон</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<ServiceResult> UpdateAsync(
            TemplateDto dto,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет последовательность смен в шаблоне
        /// </summary>
        /// <param name="model">Модель, содержащая информацию о последовательности</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<ServiceResult> UpdateSequenceAsync(
            ShiftSequencesModel model,
            CancellationToken cancellation = default
        );
    }
}