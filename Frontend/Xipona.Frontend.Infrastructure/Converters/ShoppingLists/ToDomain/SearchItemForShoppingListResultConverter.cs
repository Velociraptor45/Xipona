using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain
{
    public class SearchItemForShoppingListResultConverter :
        IToDomainConverter<SearchItemForShoppingListResultContract, SearchItemForShoppingListResult>
    {
        public SearchItemForShoppingListResult ToDomain(SearchItemForShoppingListResultContract contract)
        {
            return new SearchItemForShoppingListResult(
                contract.Id,
                contract.TypeId,
                contract.Name,
                contract.Price,
                contract.DefaultQuantity,
                contract.PriceLabel,
                contract.ItemCategoryName,
                contract.ManufacturerName,
                contract.DefaultSection.Id);
        }
    }
}