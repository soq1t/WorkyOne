using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.Domain.Configurations.Schedule.Shifts
{
    /// <summary>
    /// Конфигурация TemplatedShiftEntity для EntityFrameworkCore
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

            builder.HasMany(s => s.Sequences).WithOne(s => s.Shift).HasForeignKey(s => s.ShiftId);
        }
    }
}
