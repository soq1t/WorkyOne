using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.Repositories.Users.Configurations.Users
{
    public class UserDataEntityConfiguration : IEntityTypeConfiguration<UserDataEntity>
    {
        public void Configure(EntityTypeBuilder<UserDataEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .HasMany(u => u.Schedules)
                .WithOne(t => t.UserData)
                .HasForeignKey(t => t.UserDataId);

            builder
                .HasOne(x => x.FavoriteSchedule)
                .WithMany()
                .HasForeignKey(x => x.FavoriteScheduleId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
