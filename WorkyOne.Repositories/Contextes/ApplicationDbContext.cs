using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.Domain.Entities;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.Repositories.Contextes
{
    /// <summary>
    /// Контекст для работы с базой данных приложения
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ShiftEntity> Shifts { get; set; }

        public DbSet<RepititionEntity> ShiftRepititions { get; set; }

        public DbSet<TemplateEntity> Templates { get; set; }

        public DbSet<SingleDayShiftEntity> SingleDayShifts { get; set; }

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
