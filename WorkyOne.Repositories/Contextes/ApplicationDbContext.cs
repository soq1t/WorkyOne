﻿using Microsoft.EntityFrameworkCore;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Entities.Auth;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.Repositories.Contextes
{
    /// <summary>
    /// Контекст для работы с базой данных приложения
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ShiftEntity> Shifts { get; set; }
        public DbSet<TemplatedShiftEntity> TemplatedShifts { get; set; }
        public DbSet<DatedShiftEntity> DatedShifts { get; set; }
        public DbSet<PeriodicShiftEntity> PeriodicShifts { get; set; }

        public DbSet<ScheduleEntity> Schedules { get; set; }
        public DbSet<TemplateEntity> Templates { get; set; }

        public DbSet<DailyInfoEntity> DailyInfos { get; set; }

        public DbSet<UserDataEntity> UserDatas { get; set; }

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly)
                .Ignore<UserEntity>()
                .Ignore<SessionEntity>();

            modelBuilder
                .Entity<ShiftEntity>()
                .HasDiscriminator<ShiftType>("Type")
                .HasValue<PersonalShiftEntity>(ShiftType.Schedule)
                .HasValue<SharedShiftEntity>(ShiftType.Shared);
        }
    }
}
