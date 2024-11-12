using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Repositories.Configurations.Schedule.Common
{
    /// <summary>
    /// Конфигурация <see cref="ScheduleEntity"/> для EntityFrameworkCore
    /// </summary>
    public class ScheduleConfiguration : IEntityTypeConfiguration<ScheduleEntity>
    {
        public void Configure(EntityTypeBuilder<ScheduleEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Template);

            builder
                .HasOne(x => x.UserData)
                .WithMany(x => x.Schedules)
                .HasForeignKey(x => x.UserDataId);

            builder
                .HasMany(x => x.PersonalShifts)
                .WithOne(x => x.Schedule)
                .HasForeignKey(x => x.ScheduleId);

            builder
                .HasMany(x => x.DatedShifts)
                .WithOne(x => x.Schedule)
                .HasForeignKey(x => x.ScheduleId);

            builder
                .HasMany(x => x.PeriodicShifts)
                .WithOne(x => x.Schedule)
                .HasForeignKey(x => x.ScheduleId);

            builder
                .HasMany(s => s.Timetable)
                .WithOne(d => d.Schedule)
                .HasForeignKey(x => x.ScheduleId);
        }
    }
}
