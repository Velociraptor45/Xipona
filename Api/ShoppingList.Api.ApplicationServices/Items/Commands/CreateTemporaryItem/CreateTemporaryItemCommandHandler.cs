using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.CreateTemporaryItem;

public class CreateTemporaryItemCommandHandler : ICommandHandler<CreateTemporaryItemCommand, ItemReadModel>
{
    private readonly Func<CancellationToken, IItemCreationService> _itemCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateTemporaryItemCommandHandler(
        Func<CancellationToken, IItemCreationService> itemCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemCreationServiceDelegate = itemCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<ItemReadModel> HandleAsync(CreateTemporaryItemCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemCreationServiceDelegate(cancellationToken);
        var result = await service.CreateTemporaryAsync(command.TemporaryItemCreation);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}