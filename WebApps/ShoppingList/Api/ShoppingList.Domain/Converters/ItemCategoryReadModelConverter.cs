using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.SharedModels;

namespace ShoppingList.Domain.Converters
{
    public static class ItemCategoryReadModelConverter
    {
        public static ItemCategoryReadModel ToReadModel(this ItemCategory model)
        {
            return new ItemCategoryReadModel(model.Id, model.Name, model.IsDeleted);
        }
    }
}