using ShoppingList.Domain.Models;
using ShoppingList.Infrastructure.Entities;
using System.Collections.Generic;

namespace ShoppingList.Infrastructure.Converters
{
    public static class StoreItemConverter
    {
        public static StoreItem ToStoreItemDomain(this Item entity, Entities.Store store, float price)
        {
            var availability = new StoreItemAvailability(
                    new StoreId(store.Id), price);

            return new StoreItem(new StoreItemId(entity.Id),
                entity.Name,
                entity.Deleted,
                entity.Comment,
                entity.IsTemporary,
                (QuantityType)entity.QuantityType,
                entity.QuantityInPacket,
                (QuantityTypeInPacket)entity.QuantityTypeInPacket,
                new ItemCategoryId(entity.ItemCategory.Id),
                new ManufacturerId(entity.Manufacturer.Id),
                new List<StoreItemAvailability> { availability });
        }

        public static Item ToEntity(this StoreItem model)
        {
            return new Item()
            {
                Id = model.Id.Value,
                Name = model.Name,
                Deleted = model.IsDeleted,
                Comment = model.Comment,
                IsTemporary = model.IsTemporary,
                QuantityType = (int)model.QuantityType,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = (int)model.QuantityTypeInPacket,
                ItemCategoryId = model.ItemCategoryId.Value,
                ManufacturerId = model.ManufacturerId.Value
            };
        }
    }
}