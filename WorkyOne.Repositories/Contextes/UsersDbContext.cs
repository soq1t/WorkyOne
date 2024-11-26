using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkyOne.Domain.Entities.Auth;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.Repositories.Contextes
{
    /// <summary>
    /// Контекст для работы с базой данных пользователей
    /// </summary>
    public class UsersDbContext : IdentityDbContext<UserEntity>
    {
        public DbSet<SessionEntity> Sessions { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        }
    }
}
