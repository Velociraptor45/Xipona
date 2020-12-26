using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class ShoppingListItemExtensions
    {
        public static IShoppingListItem ToShoppingListItemDomain(
            this Infrastructure.Entities.Item entity, int storeId, int shoppingListId)
        {
            Infrastructure.Entities.AvailableAt priceMap = entity.AvailableAt.FirstOrDefault(map => map.StoreId == storeId);
            if (priceMap == null)
                throw new InvalidOperationException($"Item is not available at store {storeId}");

            var listMap = entity.ItemsOnLists
                .FirstOrDefault(map => map.ShoppingListId == shoppingListId);

            if (listMap == null)
                throw new DomainException(new ItemNotOnShoppingListReason(new ShoppingListId(shoppingListId),
                    new ShoppingListItemId(entity.Id)));

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
                entity.ItemCategory?.ToDomain(),
                entity.Manufacturer?.ToDomain(),
                listMap.InBasket,
                listMap.Quantity);
        }
    }
}