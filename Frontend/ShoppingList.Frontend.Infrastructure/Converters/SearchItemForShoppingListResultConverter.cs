using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Models.Index.Search;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converter
{
    public class SearchItemForShoppingListResultConverter
    {
        public SearchItemForShoppingListResult ToDomain(SearchItemForShoppingListResultContract contract)
        {
            return new SearchItemForShoppingListResult(
                contract.Id,
                contract.TypeId,
                contract.Name,
                contract.Price,
                "€",
                contract.ItemCategoryName,
                contract.ManufacturerName,
                contract.DefaultSection.Id);
        }
    }
}