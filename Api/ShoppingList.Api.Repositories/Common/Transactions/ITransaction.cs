namespace ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

public interface ITransaction : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}