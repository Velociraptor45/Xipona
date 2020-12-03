using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class ItemCategoryExtensions
    {
        public static ItemCategoryReadModel ToReadModel(this ItemCategory model)
        {
            return new ItemCategoryReadModel(model.Id, model.Name, model.IsDeleted);
        }
    }
}