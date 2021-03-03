using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class StoreReadModelExtensions
    {
        public static ShoppingListStoreContract ToContract(this ShoppingListStoreReadModel readModel)
        {
            return new ShoppingListStoreContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}