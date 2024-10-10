using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Repositories.Configurations.Schedule.Common
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
