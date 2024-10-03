using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkyOne.Domain.Entities.Schedule.Configuration
{
    /// <summary>
    /// Конфигурация ScheduleEntity для EntityFrameworkCore
    /// </summary>
    public class ScheduleConfiguration : IEntityTypeConfiguration<ScheduleEntity>
    {
        public void Configure(EntityTypeBuilder<ScheduleEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Template)
                .WithOne(x => x.Schedule)
                .HasForeignKey<ScheduleEntity>(x => x.TemplateId);

            builder
                .HasOne(x => x.UserData)
                .WithMany(x => x.Schedules)
                .HasForeignKey(x => x.UserDataId);

            builder
                .HasMany(x => x.DatedShifts)
                .WithOne(x => x.Schedule)
                .HasForeignKey(x => x.ScheduleId);

            builder
                .HasMany(x => x.PeriodicShifts)
                .WithOne(x => x.Schedule)
                .HasForeignKey(x => x.ScheduleId);
        }
    }
}
