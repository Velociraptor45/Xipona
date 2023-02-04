using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ItemCategories.ToDomain
{
    public class ItemCategorySearchResultConverter :
        IToDomainConverter<ItemCategorySearchResultContract, ItemCategorySearchResult>
    {
        public ItemCategorySearchResult ToDomain(ItemCategorySearchResultContract source)
        {
            return new ItemCategorySearchResult(source.Id, source.Name);
        }
    }
}