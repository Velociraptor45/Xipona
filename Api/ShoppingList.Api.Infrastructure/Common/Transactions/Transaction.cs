using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using System.Data.Common;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

public class Transaction : ITransaction
{
    private DbTransaction? _transaction;
    private readonly Func<Task> _onCloseTransactionAsync;
    private readonly ILogger<Transaction> _logger;

    public Transaction(DbTransaction transaction, Func<Task> onCloseTransactionAsync, ILogger<Transaction> logger)
    {
        _transaction = transaction;
        _onCloseTransactionAsync = onCloseTransactionAsync;
        _logger = logger;
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
            catch (Exception e)
            {
                _logger.LogError(e, () => "An error occurred while commiting a transaction");
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
            catch (Exception e)
            {
                _logger.LogError(e, () => "An error occurred while rolling back a transaction");
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