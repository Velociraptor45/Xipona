using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemCreations;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateItemWithTypes;

public class CreateItemWithTypesCommandHandler : ICommandHandler<CreateItemWithTypesCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IItemCreationService> _itemCreationServiceDelegate;

    public CreateItemWithTypesCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IItemCreationService> itemCreationServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _itemCreationServiceDelegate = itemCreationServiceDelegate;
    }

    public async Task<bool> HandleAsync(CreateItemWithTypesCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        var service = _itemCreationServiceDelegate.Invoke(cancellationToken);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        await service.CreateItemWithTypesAsync(command.Item);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}