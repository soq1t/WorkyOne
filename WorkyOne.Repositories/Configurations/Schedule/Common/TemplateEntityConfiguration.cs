using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Domain.Configurations.Schedule.Common
{
    /// <summary>
    /// Конфигурация TemplateEntity для EntityFrameworkCore
    /// </summary>
    public class TemplateEntityConfiguration : IEntityTypeConfiguration<TemplateEntity>
    {
        public void Configure(EntityTypeBuilder<TemplateEntity> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .HasMany(x => x.Shifts)
                .WithOne(x => x.Template)
                .HasForeignKey(x => x.TemplateId);

            builder
                .HasOne(x => x.Schedule)
                .WithOne(x => x.Template)
                .HasForeignKey<TemplateEntity>(x => x.ScheduleId);

            builder
                .HasMany(t => t.Sequences)
                .WithOne(s => s.Template)
                .HasForeignKey(s => s.TemplateId);
        }
    }
}
