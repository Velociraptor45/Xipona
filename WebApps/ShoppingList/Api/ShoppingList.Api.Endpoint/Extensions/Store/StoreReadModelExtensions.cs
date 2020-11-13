using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class StoreReadModelExtensions
    {
        public static StoreContract ToContract(this StoreReadModel readModel)
        {
            return new StoreContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}