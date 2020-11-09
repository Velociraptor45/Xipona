using ShoppingList.Api.Domain.Queries.SharedModels;
using System.Linq;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class ShoppingListExtensions
    {
        public static ShoppingListReadModel ToReadModel(this Models.ShoppingList model)
        {
            return new ShoppingListReadModel(model.Id, model.CompletionDate, model.Store.ToStoreReadModel(),
                model.Items.Select(item => item.ToReadModel()));
        }
    }
}