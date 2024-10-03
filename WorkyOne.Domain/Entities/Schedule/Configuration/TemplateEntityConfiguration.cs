using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkyOne.Domain.Entities.Schedule.Configuration
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
        }
    }
}
