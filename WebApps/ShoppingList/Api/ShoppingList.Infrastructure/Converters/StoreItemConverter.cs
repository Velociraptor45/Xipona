using ShoppingList.Domain.Models;
using ShoppingList.Infrastructure.Entities;
using System.Linq;

namespace ShoppingList.Infrastructure.Converters
{
    public static class StoreItemConverter
    {
        public static StoreItem ToStoreItemDomain(this Item entity)
        {
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
                entity.AvailableAt.Select(map => new StoreItemAvailability(new StoreId(map.StoreId), map.Price)));
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