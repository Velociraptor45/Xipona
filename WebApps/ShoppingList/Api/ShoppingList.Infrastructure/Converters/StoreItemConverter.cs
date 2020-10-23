using ShoppingList.Domain.Models;
using ShoppingList.Infrastructure.Entities;

namespace ShoppingList.Infrastructure.Converters
{
    public static class StoreItemConverter
    {
        public static StoreItem ToStoreItemDomain(this Item entity, Entities.Store store, float price)
        {
            return new StoreItem(new StoreItemId(entity.Id),
                entity.Name,
                entity.Deleted,
                entity.Comment,
                entity.IsTemporary,
                price,
                (QuantityType)entity.QuantityType,
                entity.QuantityInPacket,
                (QuantityTypeInPacket)entity.QuantityTypeInPacket,
                new ItemCategoryId(entity.ItemCategory.Id),
                new ManufacturerId(entity.Manufacturer.Id),
                new StoreId(store.Id));
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

        public static AvailableAt ToItemMap(this StoreItem model)
        {
            return new AvailableAt()
            {
                ItemId = model.Id.Value,
                StoreId = model.StoreId.Value,
                Price = model.Price
            };
        }
    }
}