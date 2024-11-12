using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;

namespace WorkyOne.Repositories.Configurations.Schedule.Shifts.Basic
{
    public class SharedShiftEntityConfiguration : IEntityTypeConfiguration<SharedShiftEntity>
    {
        public void Configure(EntityTypeBuilder<SharedShiftEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Schedules).WithMany(x => x.SharedShifts);
        }
    }
}
