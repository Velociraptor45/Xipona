using ShoppingList.Domain.Queries.SharedModels;
using System.Linq;

namespace ShoppingList.Domain.Converters
{
    public static class ShoppingListReadModelConverter
    {
        public static ShoppingListReadModel ToReadModel(this Models.ShoppingList model)
        {
            return new ShoppingListReadModel(model.Id, model.CompletionDate, model.Store.ToStoreReadModel(),
                model.Items.Select(item => item.ToReadModel()));
        }
    }
}