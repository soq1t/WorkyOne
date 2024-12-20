﻿using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="DatedShiftEntity"/>
    /// </summary>
    public interface IDatedShiftsRepository
        : ICrudRepository<
            DatedShiftEntity,
            EntityRequest<DatedShiftEntity>,
            PaginatedRequest<DatedShiftEntity>
        > { }
}
