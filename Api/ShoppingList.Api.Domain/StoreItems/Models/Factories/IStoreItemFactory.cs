using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
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
        IStoreItem Create(StoreItemId id, string name, bool isDeleted, string comment, bool isTemporary, QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket, IItemCategory itemCategory, IManufacturer manufacturer, IStoreItem predecessor, IEnumerable<IStoreItemAvailability> availabilities);
    }
}