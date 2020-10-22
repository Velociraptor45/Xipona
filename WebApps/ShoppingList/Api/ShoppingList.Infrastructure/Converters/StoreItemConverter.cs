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
                entity.ItemCategory.ToDomain(),
                entity.Manufacturer.ToDomain(),
                store.ToDomain());
        }
    }
}