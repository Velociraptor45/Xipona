using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.SharedModels;

namespace ShoppingList.Domain.Converters
{
    public static class StoreReadModelConverter
    {
        public static StoreReadModel ToStoreReadModel(this Store model)
        {
            return new StoreReadModel(model.Id, model.Name, model.IsDeleted);
        }
    }
}