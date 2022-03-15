using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Creations;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommandHandler : ICommandHandler<CreateManufacturerCommand, bool>
{
    private readonly Func<CancellationToken, IManufacturerCreationService> _manufacturerCrationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateManufacturerCommandHandler(
        Func<CancellationToken, IManufacturerCreationService> manufacturerCrationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _manufacturerCrationServiceDelegate = manufacturerCrationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(CreateManufacturerCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _manufacturerCrationServiceDelegate(cancellationToken);
        await service.CreateAsync(command.Name);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}