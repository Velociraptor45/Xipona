using System.Linq;

using ShoppingListModels = ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class ShoppingListExtensions
    {
        public static ShoppingListModels.ShoppingList ToDomain(this Infrastructure.Entities.ShoppingList entity)
        {
            return new ShoppingListModels.ShoppingList(
                new ShoppingListModels.ShoppingListId(entity.Id),
                entity.Store.ToDomain(),
                entity.ItemsOnList.Select(map => map.Item.ToShoppingListItemDomain(entity.StoreId, entity.Id)),
                entity.CompletionDate);
        }
    }
}