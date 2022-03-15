using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Deletions;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory;

public class DeleteItemCategoryCommandHandler : ICommandHandler<DeleteItemCategoryCommand, bool>
{
    private readonly Func<CancellationToken, IItemCategoryDeletionService> _itemCategoryDeletionServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public DeleteItemCategoryCommandHandler(
        Func<CancellationToken, IItemCategoryDeletionService> itemCategoryDeletionServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemCategoryDeletionServiceDelegate = itemCategoryDeletionServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(DeleteItemCategoryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemCategoryDeletionServiceDelegate(cancellationToken);
        await service.DeleteAsync(command.ItemCategoryId);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}