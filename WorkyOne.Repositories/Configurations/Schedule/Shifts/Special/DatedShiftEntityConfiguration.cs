using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;

namespace WorkyOne.Repositories.Configurations.Schedule.Shifts.Special
{
    /// <summary>
    /// Конфигурация DatedShiftEntity для EntityFrameworkCore
    /// </summary>
    public class DatedShiftEntityConfiguration : IEntityTypeConfiguration<DatedShiftEntity>
    {
        public void Configure(EntityTypeBuilder<DatedShiftEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Schedule)
                .WithMany(x => x.DatedShifts)
                .HasForeignKey(x => x.ScheduleId);

            builder.HasOne(x => x.Shift).WithMany(x => x.DatedShifts).HasForeignKey(x => x.ShiftId);
        }
    }
}
