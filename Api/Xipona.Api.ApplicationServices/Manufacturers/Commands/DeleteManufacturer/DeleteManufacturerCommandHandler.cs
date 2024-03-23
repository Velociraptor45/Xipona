using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Deletions;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerCommandHandler : ICommandHandler<DeleteManufacturerCommand, bool>
{
    private readonly Func<CancellationToken, IManufacturerDeletionService> _manufacturerDeletionServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public DeleteManufacturerCommandHandler(
        Func<CancellationToken, IManufacturerDeletionService> manufacturerDeletionServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _manufacturerDeletionServiceDelegate = manufacturerDeletionServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(DeleteManufacturerCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _manufacturerDeletionServiceDelegate(cancellationToken);
        await service.DeleteAsync(command.ManufacturerId);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}