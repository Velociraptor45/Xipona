﻿using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

public interface IValidator
{
    Task ValidateAsync(IEnumerable<IStoreItemAvailability> availabilities);

    Task ValidateAsync(ItemCategoryId itemCategoryId);

    Task ValidateAsync(ManufacturerId manufacturerId);
}