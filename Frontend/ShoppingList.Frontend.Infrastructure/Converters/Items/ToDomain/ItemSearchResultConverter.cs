using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class ItemSearchResultConverter : IToDomainConverter<SearchItemResultContract, ItemSearchResult>
    {
        public ItemSearchResult ToDomain(SearchItemResultContract contract)
        {
            return new ItemSearchResult(
                contract.ItemId,
                contract.ItemName);
        }
    }
}