﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Deletions;

public class ItemDeletionService : IItemDeletionService
{
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly CancellationToken _cancellationToken;

    public ItemDeletionService(
        IItemRepository itemRepository,
        IShoppingListRepository shoppingListRepository,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _shoppingListRepository = shoppingListRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task DeleteAsync(ItemId itemId)
    {
        var item = await _itemRepository.FindByAsync(itemId, _cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        item.Delete();
        var listsWithItem = (await _shoppingListRepository.FindActiveByAsync(item.Id, _cancellationToken)).ToList();

        await _itemRepository.StoreAsync(item, _cancellationToken);

        foreach (var list in listsWithItem)
        {
            _cancellationToken.ThrowIfCancellationRequested();

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
    }
}