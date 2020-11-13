using System.Linq;

namespace ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class ShoppingListExtensions
    {
        public static Domain.Models.ShoppingList ToDomain(this Infrastructure.Entities.ShoppingList entity)
        {
            return new Domain.Models.ShoppingList(
                new Domain.Models.ShoppingListId(entity.Id),
                entity.Store.ToDomain(),
                entity.ItemsOnList.Select(map => map.Item.ToShoppingListItemDomain(entity.StoreId, entity.Id)),
                entity.CompletionDate);
        }
    }
}