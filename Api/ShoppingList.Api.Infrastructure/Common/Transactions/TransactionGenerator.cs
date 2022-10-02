using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

public class TransactionGenerator : ITransactionGenerator
{
    private readonly object _lockObject = new();
    private readonly IList<DbContext> _dbContexts;
    private readonly DbConnection _connection;
    private readonly ILogger<Transaction> _transactionLogger;

    public TransactionGenerator(IList<DbContext> dbContexts, DbConnection connection,
        ILogger<Transaction> transactionLogger)
    {
        _dbContexts = dbContexts;
        _connection = connection;
        _transactionLogger = transactionLogger;
    }

    public Task<ITransaction> GenerateAsync(CancellationToken cancellationToken)
    {
        lock (_lockObject)
        {
            var dbTransaction = _connection.BeginTransactionAsync(cancellationToken)
                .GetAwaiter().GetResult();
            foreach (var database in _dbContexts.Select(ctx => ctx.Database))
            {
                if (database.CurrentTransaction != null)
                    throw new InvalidOperationException("Transaction already open");

                database.UseTransactionAsync(dbTransaction, cancellationToken)
                    .GetAwaiter().GetResult();
            }

            return Task.FromResult(
                new Transaction(dbTransaction, async () => await RemoveTransactions(cancellationToken), _transactionLogger)
                    as ITransaction);
        }
    }

    private async Task RemoveTransactions(CancellationToken cancellationToken)
    {
        foreach (var database in _dbContexts.Select(ctx => ctx.Database))
        {
            if (database.CurrentTransaction == null)
                continue;

            await database.UseTransactionAsync(null, cancellationToken);
        }
    }
}