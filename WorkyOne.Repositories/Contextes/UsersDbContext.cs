using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.Repositories.Contextes
{
    /// <summary>
    /// Контекст для работы с базой данных пользователей
    /// </summary>
    public class UsersDbContext : IdentityDbContext<UserEntity>
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options) { }
    }
}
