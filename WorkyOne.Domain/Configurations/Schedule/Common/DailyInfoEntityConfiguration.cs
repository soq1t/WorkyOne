using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Domain.Configurations.Schedule.Common
{
    /// <summary>
    /// Конфигурация DailyInfoEntity для EntityFrameworkCore
    /// </summary>
    public sealed class DailyInfoEntityConfiguration : IEntityTypeConfiguration<DailyInfoEntity>
    {
        public void Configure(EntityTypeBuilder<DailyInfoEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder
                .HasOne(c => c.Schedule)
                .WithMany(s => s.Timetable)
                .HasForeignKey(c => c.ScheduleId);
        }
    }
}
