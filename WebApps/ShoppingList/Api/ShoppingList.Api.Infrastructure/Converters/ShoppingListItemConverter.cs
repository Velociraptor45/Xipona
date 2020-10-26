using System;
using System.Linq;
using Models = ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Infrastructure.Converters
{
    public static class ShoppingListItemConverter
    {
        public static Models.ShoppingListItem ToShoppingListItemDomain(this Entities.Item entity, int storeId, int shoppingListId)
        {
            Entities.AvailableAt priceMap = entity.AvailableAt.FirstOrDefault(map => map.StoreId == storeId);
            if (priceMap == null)
                throw new InvalidOperationException($"Item is not available at store {storeId}");

            var listMap = entity.ItemsOnLists
                .FirstOrDefault(map => map.ShoppingListId == shoppingListId);
            if (listMap == null)
                throw new InvalidOperationException($"Item is not on list {shoppingListId}");

            return new Models.ShoppingListItem(
                new Models.ShoppingListItemId(entity.Id),
                entity.Name,
                entity.Deleted,
                entity.Comment,
                entity.IsTemporary,
                priceMap.Price,
                (Models.QuantityType)entity.QuantityType,
                entity.QuantityInPacket,
                (Models.QuantityTypeInPacket)entity.QuantityTypeInPacket,
                entity.ItemCategory.ToDomain(),
                entity.Manufacturer.ToDomain(),
                listMap.InBasket,
                listMap.Quantity);
        }

        public static Entities.Item ToEntity(this Models.ShoppingListItem model)
        {
            return new Entities.Item()
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