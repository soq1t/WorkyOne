﻿using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Interfaces.Requests.Schedule;
using WorkyOne.Domain.Requests.Users;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Users
{
    /// <summary>
    /// Репозиторий по работе с <see cref="UserDataEntity"/>
    /// </summary>
    public sealed class UserDatasRepository
        : ApplicationBaseRepository<UserDataEntity, UserDataRequest, PaginatedUserDataRequest>,
            IUserDatasRepository
    {
        public UserDatasRepository(ApplicationDbContext context)
            : base(context) { }

        public override Task<UserDataEntity?> GetAsync(
            UserDataRequest request,
            CancellationToken cancellation = default
        )
        {
            var query = _context.UserDatas.Where(request.Specification.ToExpression());
            query = QueryBuilder(query, request);

            return query.FirstOrDefaultAsync(cancellation);
        }

        public override Task<List<UserDataEntity>> GetManyAsync(
            PaginatedUserDataRequest request,
            CancellationToken cancellation = default
        )
        {
            var query = _context.UserDatas.Where(request.Specification.ToExpression());
            query = QueryBuilder(query, request);

            return query.ToListAsync(cancellation);
        }

        private static IQueryable<UserDataEntity> QueryBuilder(
            IQueryable<UserDataEntity> query,
            IUserDataRequest request
        )
        {
            if (request.IncludeFullSchedulesInfo)
            {
                query = query.Include(x => x.Schedules).ThenInclude(x => x.DatedShifts);
                query = query.Include(x => x.Schedules).ThenInclude(x => x.PeriodicShifts);
                query = query
                    .Include(x => x.Schedules)
                    .ThenInclude(x => x.Template)
                    .ThenInclude(x => x.Sequences);
                query = query
                    .Include(x => x.Schedules)
                    .ThenInclude(x => x.Template)
                    .ThenInclude(x => x.Shifts);

                return query;
            }

            if (request.IncludeSchedules)
            {
                query = query.Include(x => x.Schedules);
                return query;
            }

            return query;
        }
    }
}
