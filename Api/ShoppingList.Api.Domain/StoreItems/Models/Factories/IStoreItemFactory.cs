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
        IStoreItem Create(ItemCreation itemCreation);

        IStoreItem Create(TemporaryItemCreation model);

        IStoreItem Create(ItemUpdate itemUpdate, IStoreItem predecessor);

        IStoreItem Create(ItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId, IStoreItem? predecessor,
            IEnumerable<IStoreItemAvailability> availabilities, TemporaryItemId? temporaryId);
    }
}