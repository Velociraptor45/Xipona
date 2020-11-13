using ShoppingList.Api.Contracts.Queries.AllActiveStores;
using ShoppingList.Api.Domain.Queries.AllActiveStores;

namespace ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class ActiveStoreReadModelExtensions
    {
        public static ActiveStoreContract ToContract(this ActiveStoreReadModel readModel)
        {
            return new ActiveStoreContract(readModel.Id.Value, readModel.Name, readModel.Items.Count);
        }
    }
}