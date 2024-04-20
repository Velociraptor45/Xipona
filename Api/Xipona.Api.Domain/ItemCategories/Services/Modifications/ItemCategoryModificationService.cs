﻿using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Modifications;

public class ItemCategoryModificationService : IItemCategoryModificationService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;

    public ItemCategoryModificationService(
        IItemCategoryRepository itemCategoryRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
    }

    public async Task ModifyAsync(ItemCategoryModification modification)
    {
        var itemCategory = await _itemCategoryRepository.FindActiveByAsync(modification.Id);
        if (itemCategory is null)
            throw new DomainException(new ItemCategoryNotFoundReason(modification.Id));

        itemCategory.Modify(modification);

        await _itemCategoryRepository.StoreAsync(itemCategory);
    }
}