using ShoppingList.Api.Contracts.Queries.AllActiveStores;
using ShoppingList.Api.Domain.Queries.AllActiveStores;

namespace ShoppingList.Api.Endpoint.Converters.Store
{
    public static class ActiveStoreContractConverter
    {
        public static ActiveStoreContract ToContract(this ActiveStoreReadModel readModel)
        {
            return new ActiveStoreContract(readModel.Id.Value, readModel.Name, readModel.Items.Count);
        }
    }
}