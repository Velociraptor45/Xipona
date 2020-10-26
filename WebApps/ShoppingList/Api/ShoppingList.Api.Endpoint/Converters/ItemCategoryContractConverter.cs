using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Endpoint.Converters
{
    public static class ItemCategoryContractConverter
    {
        public static ItemCategoryContract ToContract(this ItemCategoryReadModel readModel)
        {
            return new ItemCategoryContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}