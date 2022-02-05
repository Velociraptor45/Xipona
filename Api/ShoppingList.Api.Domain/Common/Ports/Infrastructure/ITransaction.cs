namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;

public interface ITransaction : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}