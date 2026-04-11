using Microsoft.EntityFrameworkCore.Storage;
using SD_Server.Domain.Base;
using SD_Server.Infra.Data.Context;

namespace SD_Server.Infra.Data.UnitOfWork
{
    public class UnitOfWork(SdServerDbContext context) : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("Nenhuma transação ativa. Chame BeginTransactionAsync antes.");

            // Os repositórios já chamaram SaveChangesAsync individualmente dentro da transação aberta.
            // Aqui apenas confirmamos (commit) todos os saves como uma unidade atômica.
            await _transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
                await _transaction.RollbackAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction is not null)
                await _transaction.DisposeAsync();
        }
    }
}
