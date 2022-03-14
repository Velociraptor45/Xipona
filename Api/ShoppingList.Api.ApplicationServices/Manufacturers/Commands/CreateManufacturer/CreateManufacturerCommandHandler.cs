using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Creations;

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