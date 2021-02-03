using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public interface IStoreItemFactory
    {
        IStoreItem Create(ItemCreation itemCreation, IItemCategory itemCategory, IManufacturer manufacturer,
            IEnumerable<IStoreItemAvailability> storeItemAvailabilities);

        IStoreItem Create(TemporaryItemCreation model, IStoreItemAvailability storeItemAvailability);

        IStoreItem Create(ItemUpdate itemUpdate, IItemCategory itemCategory, IManufacturer manufacturer, IStoreItem predecessor, IEnumerable<IStoreItemAvailability> storeItemAvailabilities);
    }
}