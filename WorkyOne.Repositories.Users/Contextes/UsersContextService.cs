using Microsoft.EntityFrameworkCore.Storage;
using WorkyOne.AppServices.Interfaces.Repositories.Context;

namespace WorkyOne.Repositories.Users.Contextes
{
    /// <summary>
    /// Сервис по взаимодействию с контекстом базы данных пользователей
    /// </summary>
    public class UsersContextService : IUsersContextService
    {
        private readonly UsersDbContext _context;
        private IDbContextTransaction? _transaction;

        public UsersContextService(UsersDbContext context)
        {
            _context = context;
        }

        public async Task CommitTransactionAsync(CancellationToken cancellation = default)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellation);
            }
        }

        public async Task CreateTransactionAsync(CancellationToken cancellation = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellation);
        }

        public void Dispose()
        {
            _transaction?.Dispose();

            _context.Dispose();
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellation = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellation);
            }
        }

        public Task SaveChangesAsync(CancellationToken cancellation = default)
        {
            return _context.SaveChangesAsync(cancellation);
        }

        public bool TransactionCreated()
        {
            return _transaction != null;
        }
    }
}
