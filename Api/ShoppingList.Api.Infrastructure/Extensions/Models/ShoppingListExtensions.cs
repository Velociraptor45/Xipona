using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class ShoppingListExtensions
    {
        public static Infrastructure.Entities.ShoppingList ToEntity(this IShoppingList model)
        {
            return new Infrastructure.Entities.ShoppingList()
            {
                Id = model.Id.Value,
                CompletionDate = model.CompletionDate,
                StoreId = model.Store.Id.Value,
                ItemsOnList = model.ToItemsOnListEntities().ToList(),
            };
        }

        public static IEnumerable<Infrastructure.Entities.ItemsOnList> ToItemsOnListEntities(
            this IShoppingList model)
        {
            return model.Items.Select(item =>
                new Infrastructure.Entities.ItemsOnList()
                {
                    ShoppingListId = model.Id.Value,
                    ItemId = item.Id.Actual.Value,
                    InBasket = item.IsInBasket,
                    Quantity = item.Quantity,
                    Item = item.ToEntity()
                });
        }
    }
}