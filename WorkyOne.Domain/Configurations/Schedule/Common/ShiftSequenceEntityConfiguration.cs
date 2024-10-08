using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Domain.Configurations.Schedule.Common
{
    /// <summary>
    /// Конфигурация ShiftSequenceEntity для EntityFrameworkCore
    /// </summary>
    public class ShiftSequenceEntityConfiguration : IEntityTypeConfiguration<ShiftSequenceEntity>
    {
        public void Configure(EntityTypeBuilder<ShiftSequenceEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder
                .HasOne(s => s.Template)
                .WithMany(t => t.Sequences)
                .HasForeignKey(s => s.TemplateId);

            builder.HasOne(s => s.Shift).WithMany(t => t.Sequences).HasForeignKey(s => s.ShiftId);
        }
    }
}
