using System.Linq;
using Models = ShoppingList.Domain.Models;

namespace ShoppingList.Infrastructure.Converters
{
    public static class ShoppingListConverter
    {
        public static Models.ShoppingList ToDomain(this Entities.ShoppingList entity)
        {
            return new Models.ShoppingList(
                new Models.ShoppingListId(entity.Id),
                entity.Store.ToDomain(),
                entity.ItemsOnList.Select(map => map.Item.ToDomain(entity.StoreId, entity.Id)),
                entity.CompletionDate);
        }

        public static Entities.ShoppingList ToEntity(this Models.ShoppingList model)
        {
            return new Entities.ShoppingList()
            {
                Id = model.Id.Value,
                CompletionDate = model.CompletionDate,
                StoreId = model.Store.Id.Value
            };
        }
    }
}