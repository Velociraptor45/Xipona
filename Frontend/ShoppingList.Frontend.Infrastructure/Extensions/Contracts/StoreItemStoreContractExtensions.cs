using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreItemStoreContractExtensions
    {
        public static StoreItemStore ToContract(this StoreItemStoreContract contrac)
        {
            return new StoreItemStore(contrac.Id, contrac.Name, contrac.Sections.Select(s => s.ToContract()));
        }
    }
}