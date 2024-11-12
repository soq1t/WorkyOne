using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Common
{
    /// <summary>
    /// Интерфейс сервиса по работе с шаблонами
    /// </summary>
    public interface ITemplateService
    {
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
        /// Создаёт шаблон в базе данных
        /// </summary>
        /// <param name="model">Модель, содержащая информацию о создаваемом шаблоне</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> CreateAsync(
            TemplateModel model,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет шаблон из базы данных
        /// </summary>
        /// <param name="id">Идентификатор шаблона</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет шаблон на основе <see cref="TemplateDto"/>
        /// </summary>
        /// <param name="dto">DTO, на основе которой обновляется шаблон</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> UpdateAsync(
            TemplateDto dto,
            CancellationToken cancellation = default
        );
    }
}
