using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkyOne.Domain.Entities.Schedule.Configuration
{
    public class SingleDayShiftEntityConfiguration : IEntityTypeConfiguration<SingleDayShiftEntity>
    {
        public void Configure(EntityTypeBuilder<SingleDayShiftEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder
                .HasOne(s => s.Template)
                .WithMany(t => t.SingleDayShifts)
                .HasForeignKey(s => s.TemplateId);

            builder
                .HasOne(s => s.Shift)
                .WithMany(s => s.SingleDayShifts)
                .HasForeignKey(s => s.ShiftId);
        }
    }
}
