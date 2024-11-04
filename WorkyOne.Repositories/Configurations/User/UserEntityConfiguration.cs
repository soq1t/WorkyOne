using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.Repositories.Configurations.User
{
    /// <summary>
    /// Конфигурация Entity Framework для <see cref="UserEntity"/>
    /// </summary>
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Sessions).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
