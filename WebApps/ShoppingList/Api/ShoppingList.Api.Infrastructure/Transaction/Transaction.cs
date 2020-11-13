using Microsoft.EntityFrameworkCore.Storage;
using ShoppingList.Api.Domain.Ports.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Infrastructure.Transaction
{
    public class Transaction : ITransaction
    {
        private IDbContextTransaction transaction;

        public Transaction(IDbContextTransaction transaction)
        {
            this.transaction = transaction ?? throw new System.ArgumentNullException(nameof(transaction));
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            ThrowIfTransactionNull();

            try
            {
                await transaction.CommitAsync(cancellationToken);
            }
            finally
            {
                try
                {
                    transaction.Dispose();
                }
#pragma warning disable S2486 // Generic exceptions should not be ignored
                catch (Exception)
                {
                    // todo log
                }
#pragma warning restore S2486 // Generic exceptions should not be ignored

                transaction = null;
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            ThrowIfTransactionNull();

            try
            {
                await transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                try
                {
                    transaction.Dispose();
                }
#pragma warning disable S2486 // Generic exceptions should not be ignored
                catch (Exception)
                {
                    // todo log
                }
#pragma warning restore S2486 // Generic exceptions should not be ignored

                transaction = null;
            }
        }

        public void Dispose()
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
}