using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.CreateItemWithTypes;

public class CreateItemWithTypesCommandHandler : ICommandHandler<CreateItemWithTypesCommand, ItemReadModel>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IItemCreationService> _itemCreationServiceDelegate;

    public CreateItemWithTypesCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IItemCreationService> itemCreationServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _itemCreationServiceDelegate = itemCreationServiceDelegate;
    }

    public async Task<ItemReadModel> HandleAsync(CreateItemWithTypesCommand command, CancellationToken cancellationToken)
    {
        var service = _itemCreationServiceDelegate.Invoke(cancellationToken);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var readModel = await service.CreateItemWithTypesAsync(command.Item);

        await transaction.CommitAsync(cancellationToken);

        return readModel;
    }
}