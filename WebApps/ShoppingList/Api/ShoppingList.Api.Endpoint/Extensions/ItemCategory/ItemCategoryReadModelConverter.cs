using ShoppingList.Api.Contracts.Queries.AllActiveItemCategories;
using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Endpoint.Extensions.ItemCategory
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