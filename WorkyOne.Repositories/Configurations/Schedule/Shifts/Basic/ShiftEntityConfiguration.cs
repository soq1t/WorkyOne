using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Abstractions.Shifts;

namespace WorkyOne.Repositories.Configurations.Schedule.Shifts.Basic
{
    public class ShiftEntityConfiguration : IEntityTypeConfiguration<ShiftEntity>
    {
        public void Configure(EntityTypeBuilder<ShiftEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.DatedShifts).WithOne(x => x.Shift).HasForeignKey(x => x.ShiftId);

            builder
                .HasMany(x => x.PeriodicShifts)
                .WithOne(x => x.Shift)
                .HasForeignKey(x => x.ShiftId);

            builder
                .HasMany(x => x.TemplatedShifts)
                .WithOne(x => x.Shift)
                .HasForeignKey(x => x.ShiftId);
        }
    }
}
