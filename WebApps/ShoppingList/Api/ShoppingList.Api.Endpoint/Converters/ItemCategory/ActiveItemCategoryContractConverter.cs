using ShoppingList.Api.Contracts.Queries.AllActiveItemCategories;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Endpoint.Converters.ItemCategory
{
    public static class ActiveItemCategoryContractConverter
    {
        public static ActiveItemCategoryContract ToActiveContract(this ItemCategoryReadModel readModel)
        {
            return new ActiveItemCategoryContract(readModel.Id.Value, readModel.Name);
        }
    }
}