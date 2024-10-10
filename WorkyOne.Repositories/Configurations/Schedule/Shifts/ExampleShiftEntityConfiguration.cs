using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.Repositories.Configurations.Schedule.Shifts
{
    /// <summary>
    /// Конфигурация ExampleShiftEntity для EntityFrameworkCore
    /// </summary>
    public class ExampleShiftEntityConfiguration : IEntityTypeConfiguration<ExampleShiftEntity>
    {
        public void Configure(EntityTypeBuilder<ExampleShiftEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
