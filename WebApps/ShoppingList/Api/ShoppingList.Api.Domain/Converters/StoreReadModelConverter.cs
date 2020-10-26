using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Domain.Converters
{
    public static class StoreReadModelConverter
    {
        public static StoreReadModel ToStoreReadModel(this Store model)
        {
            return new StoreReadModel(model.Id, model.Name, model.IsDeleted);
        }
    }
}