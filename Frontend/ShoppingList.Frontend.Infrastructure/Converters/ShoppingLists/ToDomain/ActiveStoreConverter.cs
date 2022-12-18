using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Stores.ToDomain
{
    public class ActiveStoreConverter : IToDomainConverter<ActiveStoreContract, ShoppingListStore> // todo rename
    {
        public ShoppingListStore ToDomain(ActiveStoreContract contract)
        {
            return new ShoppingListStore(contract.Id, contract.Name);
        }
    }
}