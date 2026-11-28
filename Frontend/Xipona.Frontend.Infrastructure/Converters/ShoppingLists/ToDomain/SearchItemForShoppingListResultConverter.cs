using Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain;

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
            contract.DefaultSection.Id,
            contract.IsFavorite);
    }
}