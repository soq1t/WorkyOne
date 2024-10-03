using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkyOne.Domain.Entities.Schedule.Configuration
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
