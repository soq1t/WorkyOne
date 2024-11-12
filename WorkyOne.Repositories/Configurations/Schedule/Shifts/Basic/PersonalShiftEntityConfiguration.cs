using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;

namespace WorkyOne.Repositories.Configurations.Schedule.Shifts.Basic
{
    /// <summary>
    /// Конфигурация <see cref="PersonalShiftEntity"/> для Entity Framework Core
    /// </summary>
    public class PersonalShiftEntityConfiguration : IEntityTypeConfiguration<PersonalShiftEntity>
    {
        public void Configure(EntityTypeBuilder<PersonalShiftEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Schedule)
                .WithMany(x => x.PersonalShifts)
                .HasForeignKey(x => x.ScheduleId);
        }
    }
}
