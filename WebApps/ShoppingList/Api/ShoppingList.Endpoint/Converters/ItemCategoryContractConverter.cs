using ShoppingList.Contracts.SharedContracts;
using ShoppingList.Domain.Queries.SharedModels;

namespace ShoppingList.Endpoint.Converters
{
    public static class ItemCategoryContractConverter
    {
        public static ItemCategoryContract ToContract(this ItemCategoryReadModel readModel)
        {
            return new ItemCategoryContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}