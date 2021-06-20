using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreItemSectionContractExtensions
    {
        public static StoreItemSection ToModel(this StoreItemSectionContract contract)
        {
            return new StoreItemSection(contract.Id, contract.Name, contract.SortingIndex);
        }
    }
}