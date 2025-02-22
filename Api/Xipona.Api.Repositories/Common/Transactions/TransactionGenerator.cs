using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.Extensions;
using System.Data.Common;

namespace ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

public class TransactionGenerator : ITransactionGenerator
{
    private readonly IList<DbContext> _dbContexts;
    private readonly DbConnection _connection;
    private readonly ILogger<Transaction> _transactionLogger;
    private readonly SemaphoreSlim _lockGuard;
    private readonly ILogger<TransactionGenerator> _logger;

    public TransactionGenerator(IList<DbContext> dbContexts, DbConnection connection, SemaphoreSlim lockGuard,
        ILogger<TransactionGenerator> logger, ILogger<Transaction> transactionLogger)
    {
        _dbContexts = dbContexts;
        _connection = connection;
        _transactionLogger = transactionLogger;
        _lockGuard = lockGuard;
        _logger = logger;
    }

    public async Task<ITransaction> GenerateAsync(CancellationToken cancellationToken)
    {
        await _lockGuard.WaitAsync(cancellationToken);

        try
        {
            var dbTransaction = await _connection.BeginTransactionAsync(cancellationToken);
            foreach (var database in _dbContexts.Select(ctx => ctx.Database))
            {
                if (database.CurrentTransaction != null)
                {
                    _logger.LogError("Transaction is already open");
                    throw new InvalidOperationException("Transaction is already open");
                }

                await database.UseTransactionAsync(dbTransaction, cancellationToken);
            }

            return new Transaction(
                dbTransaction,
                async () => await RemoveTransactions(cancellationToken),
                _transactionLogger);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception occurred while creating a transaction");
            throw;
        }
        finally
        {
            _lockGuard.Release();
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