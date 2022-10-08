using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;

public class ModifyManufacturerCommandHandler : ICommandHandler<ModifyManufacturerCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IManufacturerModificationService> _manufacturerModificationServiceDelegate;

    public ModifyManufacturerCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IManufacturerModificationService> manufacturerModificationServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _manufacturerModificationServiceDelegate = manufacturerModificationServiceDelegate;
    }

    public async Task<bool> HandleAsync(ModifyManufacturerCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _manufacturerModificationServiceDelegate(cancellationToken);
        await service.ModifyAsync(command.Modification);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}