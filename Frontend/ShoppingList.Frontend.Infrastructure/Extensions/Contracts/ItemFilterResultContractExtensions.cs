using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ItemFilterResultContractExtensions
    {
        public static SearchItemResult ToModel(this SearchItemResultContract contract)
        {
            return new SearchItemResult(contract.ItemId, contract.ItemName);
        }
    }
}