namespace ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

public interface ITransactionGenerator
{
    Task<ITransaction> GenerateAsync(CancellationToken cancellationToken);
}