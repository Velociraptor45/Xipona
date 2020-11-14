using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class StoreItemAvailabilityReadModelExtensions
    {
        public static StoreItemAvailabilityContract ToContract(this StoreItemAvailabilityReadModel readModel)
        {
            return new StoreItemAvailabilityContract(readModel.StoreId.Value, readModel.Price);
        }
    }
}