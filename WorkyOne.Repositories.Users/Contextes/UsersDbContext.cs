using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Entities.Auth;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Repositories.Users.Configurations.Auth;
using WorkyOne.Repositories.Users.Configurations.Users;

namespace WorkyOne.Repositories.Users.Contextes
{
    /// <summary>
    /// Контекст для работы с базой данных пользователей
    /// </summary>
    public class UsersDbContext : IdentityDbContext<UserEntity>
    {
        public DbSet<SessionEntity> Sessions { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly)
                .Ignore<DailyInfoEntity>()
                .Ignore<ScheduleEntity>()
                .Ignore<TemplateEntity>()
                .Ignore<PersonalShiftEntity>()
                .Ignore<SharedShiftEntity>()
                .Ignore<ShiftEntity>()
                .Ignore<DatedShiftEntity>()
                .Ignore<PeriodicShiftEntity>()
                .Ignore<TemplatedShiftEntity>();
        }
    }
}
