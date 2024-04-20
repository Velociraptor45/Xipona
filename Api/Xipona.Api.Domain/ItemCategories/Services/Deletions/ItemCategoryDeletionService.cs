﻿using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Ports;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Deletions;

public class ItemCategoryDeletionService : IItemCategoryDeletionService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListRepository _shoppingListRepository;

    public ItemCategoryDeletionService(
        IItemCategoryRepository itemCategoryRepository,
        IItemRepository itemRepository,
        IShoppingListRepository shoppingListRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _itemRepository = itemRepository;
        _shoppingListRepository = shoppingListRepository;
    }

    public async Task DeleteAsync(ItemCategoryId itemCategoryId)
    {
        var category = await _itemCategoryRepository.FindActiveByAsync(itemCategoryId);
        if (category == null)
            return;

        category.Delete();

        var items = (await _itemRepository.FindActiveByAsync(itemCategoryId))
            .ToList();

        foreach (var item in items)
        {
            var lists = await _shoppingListRepository.FindActiveByAsync(item.Id);
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

                await _shoppingListRepository.StoreAsync(list);
            }
            item.Delete();
            await _itemRepository.StoreAsync(item);
        }
        await _itemCategoryRepository.StoreAsync(category);
    }
}