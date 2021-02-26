using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class StoreSectionContractExtensions
    {
        public static IStoreSection ToDomain(this StoreSectionContract contract)
        {
            return new StoreSection(
                new StoreSectionId(contract.Id),
                contract.Name,
                contract.SortingIndex,
                contract.IsDefaultSection);
        }
    }
}