using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreSectionContractExtensions
    {
        public static StoreItemSection ToStoreItemSectionModel(this StoreSectionContract contract)
        {
            return new StoreItemSection(
                contract.Id,
                contract.Name,
                contract.SortingIndex);
        }
    }
}