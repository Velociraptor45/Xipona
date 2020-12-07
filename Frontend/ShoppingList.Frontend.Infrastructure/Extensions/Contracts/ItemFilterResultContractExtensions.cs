using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ItemFilterResultContractExtensions
    {
        public static ItemFilterResult ToModel(this ItemFilterResultContract contract)
        {
            return new ItemFilterResult(contract.ItemId, contract.ItemName);
        }
    }
}