using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class UpdateStoreContractExtensions
    {
        public static StoreUpdate ToDomain(this UpdateStoreContract contract)
        {
            return new StoreUpdate(
                new StoreId(contract.Id),
                contract.Name,
                contract.Sections.Select(s => s.ToDomain()));
        }
    }
}