﻿using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

public interface IItemQueryService
{
    Task<StoreItemReadModel> GetAsync(ItemId itemId);
}