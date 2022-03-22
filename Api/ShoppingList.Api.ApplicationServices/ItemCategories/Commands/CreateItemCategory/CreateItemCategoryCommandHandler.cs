using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Creations;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryCommandHandler : ICommandHandler<CreateItemCategoryCommand, bool>
{
    private readonly Func<CancellationToken, IItemCategoryCreationService> _itemCategoryCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateItemCategoryCommandHandler(
        Func<CancellationToken, IItemCategoryCreationService> itemCategoryCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemCategoryCreationServiceDelegate = itemCategoryCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(CreateItemCategoryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemCategoryCreationServiceDelegate(cancellationToken);
        await service.CreateAsync(command.Name);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}