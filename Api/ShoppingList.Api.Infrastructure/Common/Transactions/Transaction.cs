using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using System.Data.Common;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

public class Transaction : ITransaction
{
    private DbTransaction? transaction;

    public Transaction(DbTransaction transaction)
    {
        this.transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        ThrowIfTransactionNull();

        try
        {
            await transaction!.CommitAsync(cancellationToken);
        }
        finally
        {
            try
            {
                transaction!.Dispose();
            }
            catch (Exception)
            {
                // todo log
            }

            transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        ThrowIfTransactionNull();

        try
        {
            await transaction!.RollbackAsync(cancellationToken);
        }
        finally
        {
            try
            {
                transaction!.Dispose();
            }
            catch (Exception)
            {
                // todo log
            }

            transaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (transaction == null)
            return;
        RollbackAsync(default).GetAwaiter().GetResult();
    }

    private void ThrowIfTransactionNull()
    {
        if (transaction == null)
            throw new InvalidOperationException("Transaction is null");
    }
}