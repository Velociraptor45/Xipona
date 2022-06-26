using System.Data.Common;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

public class Transaction : ITransaction
{
    private DbTransaction? _transaction;
    private readonly Func<Task> _onCloseTransactionAsync;

    public Transaction(DbTransaction transaction, Func<Task> onCloseTransactionAsync)
    {
        _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        _onCloseTransactionAsync = onCloseTransactionAsync;
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
                await _transaction!.DisposeAsync();
            }
            catch (Exception)
            {
                // todo log
            }

            _transaction = null;
            await _onCloseTransactionAsync.Invoke();
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
                await _transaction!.DisposeAsync();
            }
            catch (Exception)
            {
                // todo log
            }

            _transaction = null;
            await _onCloseTransactionAsync.Invoke();
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