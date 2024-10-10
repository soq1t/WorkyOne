using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common
{
    /// <summary>
    /// Интерфейс репозитория для работы с <see cref="TemplateEntity"/>
    /// </summary>
    public interface ITemplatesRepository : IEntityRepository<TemplateEntity, TemplateRequest> { }
}
