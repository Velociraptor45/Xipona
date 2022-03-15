namespace ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

public interface ITransaction : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}