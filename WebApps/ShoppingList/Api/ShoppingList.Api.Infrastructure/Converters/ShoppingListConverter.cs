using System.Collections.Generic;
using System.Linq;
using Models = ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Infrastructure.Converters
{
    public static class ShoppingListConverter
    {
        public static Models.ShoppingList ToDomain(this Entities.ShoppingList entity)
        {
            return new Models.ShoppingList(
                new Models.ShoppingListId(entity.Id),
                entity.Store.ToDomain(),
                entity.ItemsOnList.Select(map => map.Item.ToShoppingListItemDomain(entity.StoreId, entity.Id)),
                entity.CompletionDate);
        }

        public static Entities.ShoppingList ToEntity(this Models.ShoppingList model)
        {
            return new Entities.ShoppingList()
            {
                Id = model.Id.Value,
                CompletionDate = model.CompletionDate,
                StoreId = model.Store.Id.Value,
                ItemsOnList = model.ToItemsOnListEntities().ToList(),
            };
        }

        public static IEnumerable<Entities.ItemsOnList> ToItemsOnListEntities(this Models.ShoppingList model)
        {
            return model.Items.Select(item =>
                new Entities.ItemsOnList()
                {
                    ShoppingListId = model.Id.Value,
                    ItemId = item.Id.Value,
                    InBasket = item.IsInBasket,
                    Quantity = item.Quantity,
                    Item = item.ToEntity()
                });
        }
    }
}