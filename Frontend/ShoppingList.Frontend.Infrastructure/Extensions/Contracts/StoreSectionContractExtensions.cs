using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
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

        public static StoreItemSection ToStoreItemSectionModel(this StoreSectionContract contract)
        {
            return new StoreItemSection(
                contract.Id,
                contract.Name,
                contract.SortingIndex);
        }
    }
}