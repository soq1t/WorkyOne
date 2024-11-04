using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkyOne.Domain.Entities.Auth;

namespace WorkyOne.Repositories.Configurations.Auth
{
    /// <summary>
    /// Конфигурация Entity Framework для <see cref="SessionEntity"/>
    /// </summary>
    public class SessionEntityConfiguration : IEntityTypeConfiguration<SessionEntity>
    {
        public void Configure(EntityTypeBuilder<SessionEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Token).IsUnique();

            builder.HasOne(x => x.User).WithMany(x => x.Sessions).HasForeignKey(x => x.UserId);
        }
    }
}
