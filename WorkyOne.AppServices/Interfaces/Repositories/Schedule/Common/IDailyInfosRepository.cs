﻿using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="DailyInfoEntity"/>
    /// </summary>
    public interface IDailyInfosRepository
        : ICrudRepository<
            DailyInfoEntity,
            EntityRequest<DailyInfoEntity>,
            PaginatedRequest<DailyInfoEntity>
        >,
            IDeleteByConditionRepository<DailyInfoEntity> { }
}
