using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.Domain.Entities;
using WorkyOne.Domain.Entities.Schedule;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.Repositories.Contextes
{
    /// <summary>
    /// Контекст для работы с базой данных приложения
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TemplatedShiftEntity> TemplatedShifts { get; set; }
        public DbSet<DatedShiftEntity> DatedShifts { get; set; }
        public DbSet<PeriodicShiftEntity> PeriodicShifts { get; set; }
        public DbSet<ExampleShiftEntity> ExampleShifts { get; set; }

        public DbSet<ScheduleEntity> Schedules { get; set; }
        public DbSet<TemplateEntity> Templates { get; set; }

        public DbSet<DailyInfoEntity> DailyInfos { get; set; }

        public DbSet<UserDataEntity> UserDatas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
