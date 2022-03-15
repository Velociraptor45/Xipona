namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

public interface ITransactionGenerator
{
    Task<ITransaction> GenerateAsync(CancellationToken cancellationToken);
}