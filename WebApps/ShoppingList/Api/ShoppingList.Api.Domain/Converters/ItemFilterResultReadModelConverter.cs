using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.ItemFilterResults;

namespace ShoppingList.Api.Domain.Converters
{
    public static class ItemFilterResultReadModelConverter
    {
        public static ItemFilterResultReadModel ToReadModel(this StoreItem model)
        {
            return new ItemFilterResultReadModel(model.Id, model.Name);
        }
    }
}