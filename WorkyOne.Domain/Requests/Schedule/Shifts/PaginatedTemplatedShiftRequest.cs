using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Schedule.Shifts
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public sealed class PaginatedTemplatedShiftRequest : PaginatedRequest<TemplatedShiftEntity>
    {
        /// <summary>
        /// Создаёт запрос на получение множества <see cref="TemplatedShiftEntity"/> для указанного <see cref="TemplateEntity"/>
        /// </summary>
        /// <param name="templateId">Идентификатор <see cref="TemplateEntity"/></param>
        public PaginatedTemplatedShiftRequest(string? templateId)
        {
            TemplateId = templateId;
        }

        private string? _templateId;

        /// <summary>
        /// Идентификатор <see cref="TemplateEntity"/>, для которого запрашиваются <see cref="TemplatedShiftEntity"/>
        /// </summary>
        public string? TemplateId
        {
            get => _templateId;
            set
            {
                _templateId = value;
                Predicate = (x) => x.TemplateId == _templateId;
            }
        }
    }
}
