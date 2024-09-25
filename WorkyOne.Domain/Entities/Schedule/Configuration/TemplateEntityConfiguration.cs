using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkyOne.Domain.Entities.Schedule.Configuration
{
    public class TemplateEntityConfiguration : IEntityTypeConfiguration<TemplateEntity>
    {
        public void Configure(EntityTypeBuilder<TemplateEntity> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .HasOne(t => t.UserData)
                .WithMany(u => u.Templates)
                .HasForeignKey(t => t.UserDataId);

            builder
                .HasMany(t => t.SingleDayShifts)
                .WithOne(s => s.Template)
                .HasForeignKey(s => s.TemplateId);
        }
    }
}
