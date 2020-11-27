using ShoppingList.Api.Infrastructure.Extensions.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class ShoppingListExtensions
    {
        public static Infrastructure.Entities.ShoppingList ToEntity(this Domain.Models.ShoppingList model)
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
            this Domain.Models.ShoppingList model)
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