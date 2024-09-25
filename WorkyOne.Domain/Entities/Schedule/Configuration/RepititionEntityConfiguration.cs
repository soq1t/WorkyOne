using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkyOne.Domain.Entities.Schedule.Configuration
{
    public class RepititionEntityConfiguration : IEntityTypeConfiguration<RepititionEntity>
    {
        public void Configure(EntityTypeBuilder<RepititionEntity> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasOne(r => r.Shift).WithMany(s => s.Repetitions).HasForeignKey(r => r.ShiftId);
        }
    }
}
