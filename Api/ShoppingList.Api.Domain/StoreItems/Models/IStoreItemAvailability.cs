﻿using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public interface IStoreItemAvailability
{
    StoreId StoreId { get; }
    Price Price { get; }
    SectionId DefaultSectionId { get; }
}