using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Common;

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

        /// <summary>
        /// Обновляет последовательность смен в шаблоне
        /// </summary>
        /// <param name="model">Модель, содержащая информацию о последовательности</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> UpdateSequenceAsync(
            ShiftSequencesModel model,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает последовательность смен в шаблоне
        /// </summary>
        /// <param name="templateId">Идентификатор расписания, для которого запрашивается последовательность</param>
        /// <param name="request">Запрос, содержащий информацию о пагинации</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <returns></returns>
        public Task<List<ShiftSequenceDto>> GetSequencesAsync(
            string templateId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        );
    }
}
