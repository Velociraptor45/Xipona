using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;

public class ItemCategoryModificationService : IItemCategoryModificationService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly CancellationToken _cancellationToken;

    public ItemCategoryModificationService(IItemCategoryRepository itemCategoryRepository,
        CancellationToken cancellationToken)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task ModifyAsync(ItemCategoryModification modification)
    {
        var itemCategory = await _itemCategoryRepository.FindByAsync(modification.Id, _cancellationToken);
        if (itemCategory is null)
            throw new DomainException(new ItemCategoryNotFoundReason(modification.Id));

        itemCategory.Modify(modification);

        await _itemCategoryRepository.StoreAsync(itemCategory, _cancellationToken);
    }
}