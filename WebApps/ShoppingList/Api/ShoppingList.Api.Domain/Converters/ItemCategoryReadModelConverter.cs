using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Domain.Converters
{
    public static class ItemCategoryReadModelConverter
    {
        public static ItemCategoryReadModel ToReadModel(this ItemCategory model)
        {
            return new ItemCategoryReadModel(model.Id, model.Name, model.IsDeleted);
        }
    }
}