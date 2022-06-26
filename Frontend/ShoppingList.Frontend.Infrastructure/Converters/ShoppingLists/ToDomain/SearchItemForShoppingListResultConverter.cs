using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain
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
                "€",
                contract.ItemCategoryName,
                contract.ManufacturerName,
                contract.DefaultSection.Id);
        }
    }
}