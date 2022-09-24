﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;

public class ItemValidationService : IItemValidationService
{
    private readonly IItemRepository _itemRepository;
    private readonly CancellationToken _cancellationToken;

    public ItemValidationService(IItemRepository itemRepository, CancellationToken _cancellationToken)
    {
        _itemRepository = itemRepository;
        this._cancellationToken = _cancellationToken;
    }

    public async Task ValidateAsync(ItemId itemId, ItemTypeId? itemTypeId)
    {
        var result = await _itemRepository.FindActiveByAsync(itemId, itemTypeId, _cancellationToken);

        if (result is not null)
            return;

        if (itemTypeId is null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        throw new DomainException(new ItemTypeNotFoundReason(itemId, itemTypeId.Value));
    }
}