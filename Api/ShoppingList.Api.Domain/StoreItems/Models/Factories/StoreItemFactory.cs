using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class StoreItemFactory : IStoreItemFactory
    {
        public IStoreItem Create(ItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId, IStoreItem? predecessor,
            IEnumerable<IStoreItemAvailability> availabilities, TemporaryItemId? temporaryId)
        {
            var item = new StoreItem(
                id,
                name,
                isDeleted,
                comment,
                isTemporary,
                quantityType,
                quantityInPacket,
                quantityTypeInPacket,
                itemCategoryId,
                manufacturerId,
                availabilities,
                temporaryId);

            if (predecessor != null)
                item.SetPredecessor(predecessor);

            return item;
        }

        public IStoreItem Create(ItemId id, string name, bool isDeleted, string comment,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, IStoreItem? predecessor,
            IEnumerable<IItemType> itemTypes)
        {
            var item = new StoreItem(
                id,
                name,
                isDeleted,
                comment,
                quantityType,
                quantityInPacket,
                quantityTypeInPacket,
                itemCategoryId,
                manufacturerId,
                itemTypes);

            if (predecessor != null)
                item.SetPredecessor(predecessor);

            return item;
        }

        public IStoreItem Create(ItemCreation itemCreation)
        {
            return new StoreItem(new ItemId(0),
                itemCreation.Name,
                false,
                itemCreation.Comment,
                false,
                itemCreation.QuantityType,
                itemCreation.QuantityInPacket,
                itemCreation.QuantityTypeInPacket,
                itemCreation.ItemCategoryId,
                itemCreation.ManufacturerId,
                itemCreation.Availabilities,
                null);
        }

        public IStoreItem Create(TemporaryItemCreation model)
        {
            return new StoreItem(
                new ItemId(0),
                model.Name,
                false,
                string.Empty,
                true,
                QuantityType.Unit,
                1,
                QuantityTypeInPacket.Unit,
                null,
                null,
                model.Availability.ToMonoList(),
                new TemporaryItemId(model.ClientSideId));
        }

        public IStoreItem Create(ItemUpdate itemUpdate, IStoreItem predecessor)
        {
            var model = new StoreItem(
                new ItemId(0),
                itemUpdate.Name,
                isDeleted: false,
                itemUpdate.Comment,
                isTemporary: false,
                itemUpdate.QuantityType,
                itemUpdate.QuantityInPacket,
                itemUpdate.QuantityTypeInPacket,
                itemUpdate.ItemCategoryId,
                itemUpdate.ManufacturerId,
                itemUpdate.Availabilities,
                null);

            model.SetPredecessor(predecessor);
            return model;
        }
    }
}