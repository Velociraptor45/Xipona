using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converter
{
    public class SearchItemResultConverter
    {
        public SearchItemResult ToDomain(SearchItemResultContract contract)
        {
            return new SearchItemResult(
                contract.ItemId,
                contract.ItemName);
        }
    }
}