using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using System.Data.Common;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

public class TransactionGenerator : ITransactionGenerator
{
    private readonly object lockObject = new object();
    private readonly IList<DbContext> dbContexts;
    private readonly DbConnection connection;

    public TransactionGenerator(IList<DbContext> dbContexts, DbConnection connection)
    {
        this.dbContexts = dbContexts;
        this.connection = connection;
    }

    public Task<ITransaction> GenerateAsync(CancellationToken cancellationToken)
    {
        lock (lockObject)
        {
            var dbTransaction = connection.BeginTransactionAsync(cancellationToken)
                .GetAwaiter().GetResult();
            foreach (var database in dbContexts.Select(ctx => ctx.Database))
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