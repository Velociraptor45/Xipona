using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;

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
        ArgumentNullException.ThrowIfNull(command);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _manufacturerDeletionServiceDelegate(cancellationToken);
        await service.DeleteAsync(command.ManufacturerId);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}