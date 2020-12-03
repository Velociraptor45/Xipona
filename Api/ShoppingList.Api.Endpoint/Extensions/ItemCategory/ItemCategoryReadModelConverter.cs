using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategory.Queries.AllActiveItemCategories;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ItemCategory
{
    public static class ItemCategoryReadModelConverter
    {
        public static ActiveItemCategoryContract ToActiveContract(this ItemCategoryReadModel readModel)
        {
            return new ActiveItemCategoryContract(readModel.Id.Value, readModel.Name);
        }

        public static ItemCategoryContract ToContract(this ItemCategoryReadModel readModel)
        {
            return new ItemCategoryContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}