﻿using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Validations;

public class ItemCategoryValidationService : IItemCategoryValidationService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;

    public ItemCategoryValidationService(IItemCategoryRepository itemCategoryRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
    }

    public async Task ValidateAsync(ItemCategoryId itemCategoryId)
    {
        IItemCategory? itemCategory = await _itemCategoryRepository
            .FindActiveByAsync(itemCategoryId);

        if (itemCategory == null)
            throw new DomainException(new ItemCategoryNotFoundReason(itemCategoryId));
    }
}