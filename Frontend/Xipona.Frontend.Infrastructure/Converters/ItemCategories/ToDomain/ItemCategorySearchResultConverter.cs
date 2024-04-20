using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ItemCategories.ToDomain
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