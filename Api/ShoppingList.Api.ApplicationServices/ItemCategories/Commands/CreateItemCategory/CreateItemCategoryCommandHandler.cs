using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Creations;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryCommandHandler : ICommandHandler<CreateItemCategoryCommand, IItemCategory>
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

    public async Task<IItemCategory> HandleAsync(CreateItemCategoryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemCategoryCreationServiceDelegate(cancellationToken);
        var result = await service.CreateAsync(command.Name);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}