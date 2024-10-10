using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.Domain.Configurations.Schedule.Shifts
{
    /// <summary>
    /// Конфигурация PeriodicShiftEntity для EntityFrameworkCore
    /// </summary>
    public class PeriodicShiftEntityConfiguration : IEntityTypeConfiguration<PeriodicShiftEntity>
    {
        public void Configure(EntityTypeBuilder<PeriodicShiftEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Schedule)
                .WithMany(x => x.PeriodicShifts)
                .HasForeignKey(x => x.ScheduleId);
        }
    }
}
