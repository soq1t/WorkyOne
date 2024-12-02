using Microsoft.EntityFrameworkCore.Storage;
using WorkyOne.AppServices.Interfaces.Repositories.Context;

namespace WorkyOne.Repositories.Contextes
{
    /// <summary>
    /// Сервис по взаимодействию с контекстом приложения
    /// </summary>
    public class ApplicationContextService : IApplicationContextService
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public ApplicationContextService(ApplicationDbContext context)
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
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellation);
            }

            _transaction = await _context.Database.BeginTransactionAsync(cancellation);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
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
    }
}
