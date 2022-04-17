using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Deletions;

public class ItemCategoryDeletionService : IItemCategoryDeletionService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly CancellationToken _cancellationToken;

    public ItemCategoryDeletionService(
        IItemCategoryRepository itemCategoryRepository,
        IItemRepository itemRepository,
        IShoppingListRepository shoppingListRepository,
        CancellationToken cancellationToken)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _itemRepository = itemRepository;
        _shoppingListRepository = shoppingListRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task DeleteAsync(ItemCategoryId itemCategoryId)
    {
        var category = await _itemCategoryRepository.FindByAsync(itemCategoryId, _cancellationToken);
        if (category == null)
            throw new DomainException(new ItemCategoryNotFoundReason(itemCategoryId));

        category.Delete();

        var storeItems = (await _itemRepository.FindActiveByAsync(itemCategoryId, _cancellationToken))
            .ToList();

        foreach (var item in storeItems)
        {
            var lists = await _shoppingListRepository.FindActiveByAsync(item.Id, _cancellationToken);
            foreach (var list in lists)
            {
                if (item.HasItemTypes)
                {
                    foreach (var type in item.ItemTypes)
                    {
                        list.RemoveItem(item.Id, type.Id);
                    }
                }
                else
                {
                    list.RemoveItem(item.Id);
                }

                await _shoppingListRepository.StoreAsync(list, _cancellationToken);
            }
            item.Delete();
            await _itemRepository.StoreAsync(item, _cancellationToken);
        }
        await _itemCategoryRepository.StoreAsync(category, _cancellationToken);
    }
}