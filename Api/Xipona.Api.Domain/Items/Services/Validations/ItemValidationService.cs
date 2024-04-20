﻿using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Validations;

public class ItemValidationService : IItemValidationService
{
    private readonly IItemRepository _itemRepository;

    public ItemValidationService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task ValidateAsync(ItemId itemId, ItemTypeId? itemTypeId)
    {
        var result = await _itemRepository.FindActiveByAsync(itemId, itemTypeId);

        if (result is not null)
            return;

        if (itemTypeId is null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        throw new DomainException(new ItemTypeNotFoundReason(itemId, itemTypeId.Value));
    }
}