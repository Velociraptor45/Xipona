using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using System.Data.Common;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

public class Transaction : ITransaction
{
    private DbTransaction? _transaction;

    public Transaction(DbTransaction transaction)
    {
        _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        ThrowIfTransactionNull();

        try
        {
            await _transaction!.CommitAsync(cancellationToken);
        }
        finally
        {
            try
            {
                _transaction!.Dispose();
            }
            catch (Exception)
            {
                // todo log
            }

            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        ThrowIfTransactionNull();

        try
        {
            await _transaction!.RollbackAsync(cancellationToken);
        }
        finally
        {
            try
            {
                _transaction!.Dispose();
            }
            catch (Exception)
            {
                // todo log
            }

            _transaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_transaction == null)
            return;
        RollbackAsync(default).GetAwaiter().GetResult();
    }

    private void ThrowIfTransactionNull()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction is null");
    }
}