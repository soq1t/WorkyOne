using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;

namespace WorkyOne.Repositories.Configurations.Schedule.Shifts.Special
{
    /// <summary>
    /// Конфигурация <see cref="TemplatedShiftEntity"/> для EntityFrameworkCore
    /// </summary>
    public class TemplatedShiftEntityConfiguration : IEntityTypeConfiguration<TemplatedShiftEntity>
    {
        public void Configure(EntityTypeBuilder<TemplatedShiftEntity> builder)
        {
            builder.HasKey(s => s.Id);
            builder
                .HasOne(s => s.Template)
                .WithMany(s => s.Shifts)
                .HasForeignKey(s => s.TemplateId);

            builder
                .HasOne(x => x.Shift)
                .WithMany(x => x.TemplatedShifts)
                .HasForeignKey(x => x.ShiftId);
        }
    }
}
