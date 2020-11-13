using ShoppingList.Api.Domain.Ports.Infrastructure;
using ShoppingList.Api.Infrastructure.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Infrastructure.Transaction
{
    public class TransactionGenerator : ITransactionGenerator
    {
        private readonly object lockObject = new object();
        private readonly ShoppingContext dbContext;

        public TransactionGenerator(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<ITransaction> GenerateAsync(CancellationToken cancellationToken)
        {
            lock (lockObject)
            {
                var dbTransaction = dbContext.Database.BeginTransactionAsync(cancellationToken)
                    .GetAwaiter().GetResult();
                return Task.FromResult(new Transaction(dbTransaction) as ITransaction);
            }
        }
    }
}