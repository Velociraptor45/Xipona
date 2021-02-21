using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class StoreItemAvailabilityReadModelExtensions
    {
        public static StoreItemAvailabilityContract ToContract(this StoreItemAvailabilityReadModel readModel)
        {
            return new StoreItemAvailabilityContract(readModel.Store.ToContract(), readModel.Price);
        }
    }
}