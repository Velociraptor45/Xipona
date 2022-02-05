using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using System.Data.Common;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

public class TransactionGenerator : ITransactionGenerator
{
    private readonly object _lockObject = new object();
    private readonly IList<DbContext> _dbContexts;
    private readonly DbConnection _connection;

    public TransactionGenerator(IList<DbContext> dbContexts, DbConnection connection)
    {
        _dbContexts = dbContexts;
        _connection = connection;
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
            return Task.FromResult(new Transaction(dbTransaction) as ITransaction);
        }
    }
}