using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common
{
    /// <summary>
    /// Интерфейс репозитория для работы с <see cref="TemplateEntity"/>
    /// </summary>
    public interface ITemplatesRepository
        : ICrudRepository<
            TemplateEntity,
            EntityRequest<TemplateEntity>,
            PaginatedRequest<TemplateEntity>
        > { }
}
