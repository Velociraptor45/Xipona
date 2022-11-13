namespace ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

public interface ITransactionGenerator
{
    Task<ITransaction> GenerateAsync(CancellationToken cancellationToken);
}