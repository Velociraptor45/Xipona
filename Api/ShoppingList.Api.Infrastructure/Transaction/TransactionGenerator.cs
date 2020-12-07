using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Transaction
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