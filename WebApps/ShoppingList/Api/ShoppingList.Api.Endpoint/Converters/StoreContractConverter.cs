using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Endpoint.Converters
{
    public static class StoreContractConverter
    {
        public static StoreContract ToContract(this StoreReadModel readModel)
        {
            return new StoreContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}