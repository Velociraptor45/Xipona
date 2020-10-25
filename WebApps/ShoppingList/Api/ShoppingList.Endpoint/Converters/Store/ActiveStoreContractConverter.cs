using ShoppingList.Contracts.Queries.AllActiveStores;
using ShoppingList.Domain.Queries.AllActiveStores;

namespace ShoppingList.Endpoint.Converters.Store
{
    public static class ActiveStoreContractConverter
    {
        public static ActiveStoreContract ToContract(this ActiveStoreReadModel readModel)
        {
            return new ActiveStoreContract(readModel.Id.Value, readModel.Name, readModel.Items.Count);
        }
    }
}