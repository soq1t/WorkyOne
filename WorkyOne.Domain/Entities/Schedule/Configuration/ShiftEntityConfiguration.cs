using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkyOne.Domain.Entities.Schedule.Configuration
{
    public class ShiftEntityConfiguration : IEntityTypeConfiguration<ShiftEntity>
    {
        public void Configure(EntityTypeBuilder<ShiftEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasMany(s => s.Repetitions).WithOne(r => r.Shift).HasForeignKey(r => r.ShiftId);

            builder
                .HasMany(s => s.SingleDayShifts)
                .WithOne(s => s.Shift)
                .HasForeignKey(s => s.ShiftId);

            builder.HasMany(s => s.Templates).WithMany(t => t.Shifts);
        }
    }
}
