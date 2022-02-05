namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;

public interface ITransactionGenerator
{
    Task<ITransaction> GenerateAsync(CancellationToken cancellationToken);
}