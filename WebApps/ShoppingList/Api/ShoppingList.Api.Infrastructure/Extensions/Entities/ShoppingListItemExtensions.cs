using System;
using System.Linq;

namespace ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class ShoppingListItemExtensions
    {
        public static Domain.Models.ShoppingListItem ToShoppingListItemDomain(
            this Infrastructure.Entities.Item entity, int storeId, int shoppingListId)
        {
            Infrastructure.Entities.AvailableAt priceMap = entity.AvailableAt.FirstOrDefault(map => map.StoreId == storeId);
            if (priceMap == null)
                throw new InvalidOperationException($"Item is not available at store {storeId}");

            var listMap = entity.ItemsOnLists
                .FirstOrDefault(map => map.ShoppingListId == shoppingListId);
            if (listMap == null)
                throw new InvalidOperationException($"Item is not on list {shoppingListId}");

            return new Domain.Models.ShoppingListItem(
                new Domain.Models.ShoppingListItemId(entity.Id),
                entity.Name,
                entity.Deleted,
                entity.Comment,
                entity.IsTemporary,
                priceMap.Price,
                (Domain.Models.QuantityType)entity.QuantityType,
                entity.QuantityInPacket,
                (Domain.Models.QuantityTypeInPacket)entity.QuantityTypeInPacket,
                entity.ItemCategory?.ToDomain(),
                entity.Manufacturer?.ToDomain(),
                listMap.InBasket,
                listMap.Quantity);
        }
    }
}