using ShoppingList.Contracts.SharedContracts;
using ShoppingList.Domain.Queries.SharedModels;

namespace ShoppingList.Endpoint.Converters
{
    public static class StoreContractConverter
    {
        public static StoreContract ToContract(this StoreReadModel readModel)
        {
            return new StoreContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}