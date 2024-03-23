namespace ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

public interface ITransaction : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}