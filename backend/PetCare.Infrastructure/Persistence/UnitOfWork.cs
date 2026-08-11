using Microsoft.EntityFrameworkCore.Storage;
using PetCare.Domain.Interfaces;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PetsDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(PetsDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction is not null)
                    await _transaction.CommitAsync(cancellationToken);
            }
            finally
            {
                if (_transaction is not null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction is not null)
                    await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                if (_transaction is not null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }
    }
}
