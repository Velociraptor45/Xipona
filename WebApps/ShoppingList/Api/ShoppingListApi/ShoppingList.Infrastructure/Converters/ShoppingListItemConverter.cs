using ShoppingList.Domain.Models;
using ShoppingList.Infrastructure.Entities;
using System;
using System.Linq;

namespace ShoppingList.Infrastructure.Converters
{
    public static class ShoppingListItemConverter
    {
        public static ShoppingListItem ToDomain(this Item entity, int storeId, int shoppingListId)
        {
            AvailableAt priceMap = entity.AvailableAt.FirstOrDefault(map => map.StoreId == storeId);
            if (priceMap == null)
                throw new InvalidOperationException($"Item is not available at store {storeId}");

            var listMap = entity.ItemsOnLists
                .FirstOrDefault(map => map.ShoppingListId == shoppingListId);
            if (listMap == null)
                throw new InvalidOperationException($"Item is not on list {shoppingListId}");

            return new ShoppingListItem(
                new ShoppingListItemId(entity.Id),
                entity.Name,
                entity.Deleted,
                entity.Comment,
                entity.IsTemporary,
                priceMap.Price,
                (QuantityType)entity.QuantityType,
                entity.QuantityInPacket,
                (QuantityTypeInPacket)entity.QuantityTypeInPacket,
                entity.ItemCategory.ToDomain(),
                entity.Manufacturer.ToDomain(),
                listMap.InBasket,
                listMap.Quantity);
        }

        public static Item ToEntity(this ShoppingListItem model)
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
                ItemCategoryId = model.ItemCategory.Id.Value,
                ManufacturerId = model.Manufacturer.Id.Value
            };
        }
    }
}