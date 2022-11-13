using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;

public class ModifyItemCategoryCommandHandler : ICommandHandler<ModifyItemCategoryCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IItemCategoryModificationService> _itemCategoryModificationServiceDelegate;

    public ModifyItemCategoryCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IItemCategoryModificationService> itemCategoryModificationServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _itemCategoryModificationServiceDelegate = itemCategoryModificationServiceDelegate;
    }

    public async Task<bool> HandleAsync(ModifyItemCategoryCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemCategoryModificationServiceDelegate(cancellationToken);
        await service.ModifyAsync(command.Modification);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}