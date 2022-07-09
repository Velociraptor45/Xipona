using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class SearchItemResultConverter : IToDomainConverter<SearchItemResultContract, SearchItemResult>
    {
        public SearchItemResult ToDomain(SearchItemResultContract contract)
        {
            return new SearchItemResult(
                contract.ItemId,
                contract.ItemName);
        }
    }
}