using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Frontend.Models;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreSectionContractExtensions
    {
        public static StoreSection ToModel(this StoreSectionContract contract)
        {
            return new StoreSection(
                new StoreSectionId(contract.Id, Guid.NewGuid()),
                contract.Name,
                contract.SortingIndex,
                contract.IsDefautlSection);
        }
    }
}